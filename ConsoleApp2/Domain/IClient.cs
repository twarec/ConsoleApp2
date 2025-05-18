using ConsoleApp2.Models;

namespace ConsoleApp2.Domain;

public interface IClient : IObservable<IMessage>
{
    public void Send(IMessage message);
    public Task SendAsync(IMessage message);

    public Task StartAsync();
}
