public class HubProvider : Hub
{
    private static ConcurrentBag<ConnectedUser> connectedUsers = new();
    private static readonly object _lock = new();
    private static bool firstLoad = true;
    private readonly RealTimeChatDataContext _dbContext;
    public HubProvider(RealTimeChatDataContext dbContext)
    {
        _dbContext = dbContext;

        if (firstLoad)
        {
            lock (_lock)
            {
                if (firstLoad)
                {
                    var users = _dbContext.Users.Select(x =>
                        new ConnectedUser()
                        {
                            UserId = x.Id.ToString(),
                            UserName = x.Name,
                            Connected = false
                        }
                    )
                    .ToList();

                    foreach (var user in users)
                        connectedUsers.Add(user);

                    firstLoad = false;
                }
            }
        }
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context?.GetHttpContext()?.Request.Query["userId"].ToString() ?? "";
        var userName = Context?.GetHttpContext()?.Request.Query["userName"].ToString() ?? "";

        if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(userName))
            return;

        var connectedUser = connectedUsers.FirstOrDefault(u => u.UserId == userId);

        if (connectedUser is not null)
        {
            connectedUser.Connected = true;
            connectedUser.ConnectionId = Context.ConnectionId;
        }
        else
        {
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

            if (currentUser is null)
                return;

            connectedUsers.Add(
                new()
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = currentUser.Id.ToString(),
                    UserName = currentUser.Name,
                    Connected = true
                }
            );
        }

        await Clients.All.SendAsync("UsersOnline", connectedUsers);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var disconnectedUser = connectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);

        if (disconnectedUser is not null)
            disconnectedUser.Connected = false;

        await Clients.All.SendAsync("UsersOnline", connectedUsers);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageToUser(Guid targetUserId, Message message)
    {
        try
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            var connections = connectedUsers.Where(x => x.UserId == targetUserId.ToString())
                                            .Select(x => x.ConnectionId);

            foreach (var connectionId in connections)
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro não esperado!");
        }
    }
}
