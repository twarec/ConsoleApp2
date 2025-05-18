using ConsoleApp2.Models;
using Newtonsoft.Json;

namespace ConsoleApp2.Serialize;

public class JsonSerialize : ISerialize
{
    private byte[] _buffer = new byte[1024 * 1024 * 10];
    private int _offset = 0;
    private int _body = 0;

    private JsonSerializerSettings _settings = new(new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Objects,
    });

    public IEnumerable<IMessage> Deserialize(byte[] buffer, int offset, int size)
    {
        for(int i = offset; i < offset + size; i++)
        {
            _buffer[_offset] = buffer[i];

            if (_buffer[_offset] == '{')
                _body++;
            else if (_buffer[_offset] == '}')
                _body--;

            _offset++;

            if (_offset > 3)
            {
                if (_body == 0)
                {
                    var text = System.Text.Encoding.UTF8.GetString(_buffer, 0, _offset);
                    _offset = 0;
                    var value = JsonConvert.DeserializeObject<IMessage>(text, _settings);
                    if (value != null)
                        yield return value;
                }
            }
        }
    }

    public byte[] Serialize(IMessage message)
    {
        var data = JsonConvert.SerializeObject(message, _settings);
        return System.Text.Encoding.UTF8.GetBytes(data);
    }
}
