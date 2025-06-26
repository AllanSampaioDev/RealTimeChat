namespace Api.Contracts;

public class MessageRequest
{
    public string Name { get; set; }
    public Guid SenderUserId { get; set; }
    public Guid RecieverUserId { get; set; }
    public string Body { get; set; }
    public string Date { get; set; }

    public MessageRequest(string name, string body)
    {
        Name = name;
        Body = body;
        Date = DateTime.Now.ToString("G");
    }
}
