using ConsoleApp2.Models;
using ConsoleApp2.Serialize;
using NetCoreServer;
using System.Reactive.Subjects;

namespace ConsoleApp2.Domain;

public class UnixSession : UdsSession
{
    private readonly ISerialize _serialize;
    private readonly Subject<ClientMessage> _subject;
    public UnixSession(UnixServer server, ISerialize serialize, Subject<ClientMessage> subject) : base(server)
    {
        _serialize = serialize;
        _subject = subject;
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        foreach(var value in _serialize.Deserialize(buffer, (int)offset, (int)size))
        {
            _subject.OnNext(new ClientMessage(Id, value));
        }
    }
}
