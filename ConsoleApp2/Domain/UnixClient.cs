using ConsoleApp2.Models;
using ConsoleApp2.Serialize;
using NetCoreServer;
using System.Reactive.Subjects;

namespace ConsoleApp2.Domain;

public class UnixClient : UdsClient, IClient
{
    private Subject<IMessage> _subject = new();
    private ISerialize _serialize;

    public UnixClient(string path, ISerialize serialize) : base(path)
    {
        _serialize = serialize;
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        foreach(var value in _serialize.Deserialize(buffer, (int)offset, (int)size))
        {
            _subject.OnNext(value);
        }
    }

    public override long Receive(byte[] buffer, long offset, long size)
    {
        return base.Receive(buffer, offset, size);
    }

    public override long Receive(byte[] buffer)
    {
        return base.Receive(buffer);
    }

    public IDisposable Subscribe(IObserver<IMessage> observer)
    {
        return _subject.Subscribe(observer);
    }

    public Task StartAsync()
    {
        ConnectAsync();
        return Task.CompletedTask;
    }

    public void Send(IMessage message)
    {
        base.Send(_serialize.Serialize(message));
    }

    public Task SendAsync(IMessage message)
    {
        base.SendAsync(_serialize.Serialize(message));
        return Task.CompletedTask;
    }
}
