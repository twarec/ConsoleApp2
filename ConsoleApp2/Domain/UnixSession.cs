using ConsoleApp2.Models;
using ConsoleApp2.Serialize;
using NetCoreServer;
using System.Reactive.Subjects;

namespace ConsoleApp2.Domain;

public class UnixSession : UdsSession
{
    private readonly Subject<ClientMessage> _subject;

    private readonly ISerialize _serialize;
    public UnixSession(UnixServer server, ISerialize serialize, Subject<ClientMessage> subject) : base(server)
    {
        _serialize = serialize;
        _subject = subject;
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        var data = _serialize.Deserialize(buffer, (int)offset, (int)size);
        if (data != null)
            _subject.OnNext(new ClientMessage(Id, data));
    }
}
