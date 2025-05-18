using ConsoleApp2.Models;
using ConsoleApp2.Serialize;
using NetCoreServer;
using System.Reactive.Subjects;

namespace ConsoleApp2.Domain;

public class UnixServer : UdsServer, IServer
{
    private readonly ISerialize _serialize;
    private readonly Subject<ClientMessage> _subject = new();

    public UnixServer(string path, ISerialize serialize) : base(path)
    {
        _serialize = serialize;
    }

    public void Send(IMessage message)
    {
        var data = _serialize.Serialize(message);
        Multicast(data);
    }

    public Task SendAsync(IMessage message)
    {
        var data = _serialize.Serialize(message);
        Multicast(data);
        return Task.CompletedTask;
    }

    protected override UdsSession CreateSession()
    {
        return new UnixSession(this, _serialize, _subject);
    }

    public IDisposable Subscribe(IObserver<ClientMessage> observer)
    {
        return _subject.Subscribe(observer);
    }

    public Task StartAsync()
    {
        Start();
        return Task.CompletedTask;
    }
}
