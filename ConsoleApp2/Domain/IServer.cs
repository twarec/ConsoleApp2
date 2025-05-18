using ConsoleApp2.Models;

namespace ConsoleApp2.Domain;

public interface IServer : IObservable<ClientMessage>
{
    public void Send(IMessage message);
    public Task SendAsync(IMessage message);
}
