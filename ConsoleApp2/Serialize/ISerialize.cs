using ConsoleApp2.Models;

namespace ConsoleApp2.Serialize;

public interface ISerialize
{
    public byte[] Serialize(IMessage message);
    public IMessage Deserialize(byte[] buffer, int offset, int size);
}
