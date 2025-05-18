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
        var data = _serialize.Deserialize(buffer, (int)offset, (int)size);
        if (data != null)
            _subject.OnNext(data);
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

    protected override void OnConnected()
    {
        Send(_serialize.Serialize(new Test_data(Guid.NewGuid(), "feafaafa")));
    }
}
