namespace ConsoleApp2.Models;

public record struct ClientMessage(Guid ClientId, IMessage Payload);
