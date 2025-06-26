namespace Api.Domain;

public class Message
{
    public Guid Id { get; set; }
    public Guid SenderUserId { get; set; }
    public Guid RecieverUserId { get; set; }
    public string Body { get; set; }
    public string SendTime { get; set; }
}
