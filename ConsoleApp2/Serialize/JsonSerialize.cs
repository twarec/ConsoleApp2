using ConsoleApp2.Models;
using Newtonsoft.Json;

namespace ConsoleApp2.Serialize;

public class JsonSerialize : ISerialize
{
    private JsonSerializerSettings _settings = new(new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Objects,
    });

    public IMessage Deserialize(byte[] buffer, int offset, int size)
    {
        var text = System.Text.Encoding.UTF8.GetString(buffer, offset, size);
        return JsonConvert.DeserializeObject<IMessage>(text, _settings)?? throw new NullReferenceException();
    }

    public byte[] Serialize(IMessage message)
    {
        var data = JsonConvert.SerializeObject(message, _settings);
        return System.Text.Encoding.UTF8.GetBytes(data);
    }
}
