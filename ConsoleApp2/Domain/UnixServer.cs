using ConsoleApp2.Models;
using ConsoleApp2.Serialize;
using NetCoreServer;
using System.Net.Sockets;
using System.Reactive.Subjects;

namespace ConsoleApp2.Domain;

public class UnixServer : UdsServer, IServer
{
    private readonly ISerialize _serialize;

    private Subject<ClientMessage> _subject = new();
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

    public override bool Multicast(byte[] buffer)
    {
        foreach (var seddion in this.Sessions)
        {
            seddion.Value.Send(buffer);
        }
        return true;
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Chat Unix Domain Socket server caught an error with code {error}");
    }

    protected override UdsSession CreateSession()
    {
        return new UnixSession(this, _serialize, _subject);
    }

    public IDisposable Subscribe(IObserver<ClientMessage> observer)
    {
        return _subject.Subscribe(observer);
    }
}
