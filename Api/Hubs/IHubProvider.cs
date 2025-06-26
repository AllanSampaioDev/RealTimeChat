namespace Api.Hubs;

public interface IHubProvider
{
    Task RecievedMessage(MessageRequest message);
}
