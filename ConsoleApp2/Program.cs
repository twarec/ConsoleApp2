using ConsoleApp2.Domain;
using ConsoleApp2.Models;
using ConsoleApp2.Serialize;

var seriliztor = new JsonSerialize();
IServer server = new UnixServer(Path.Combine(Path.GetTempPath(), "test_46.sock"), seriliztor);
IClient client = new UnixClient(Path.Combine(Path.GetTempPath(), "test_46.sock"), seriliztor);

client.Subscribe(x =>
{
    Console.WriteLine(x);
});

server.Subscribe(x =>
{
    Console.WriteLine(x);
});

await server.StartAsync();
await client.StartAsync();

for(int i = 0; i < 1000000; i++)
{
    client.Send(new Test_data(i, Guid.NewGuid(), "HEllo world"));
}

while (true) ;


public record struct Test_data(int Index, Guid Id, string Name) : IMessage;