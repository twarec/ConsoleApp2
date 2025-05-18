namespace ConsoleApp2.Models;

public record struct ClientMessage(Guid clientId, IMessage payload)
{
    public Guid ClientId { get; } = clientId;
    public IMessage Payload { get; } = payload;
}
