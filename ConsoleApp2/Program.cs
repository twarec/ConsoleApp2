using ConsoleApp2.Domain;
using ConsoleApp2.Models;
using ConsoleApp2.Serialize;

var seriliztor = new JsonSerialize();
IServer server = new UnixServer(Path.Combine(Path.GetTempPath(), "test_30.sock"), seriliztor);
IClient client = new UnixClient(Path.Combine(Path.GetTempPath(), "test_30.sock"), seriliztor);

client.Subscribe(x =>
{
    Console.WriteLine(x);
});

server.Subscribe(x =>
{
    Console.WriteLine(x);
});

Console.WriteLine(((UnixServer)server).Start());
Console.WriteLine(((UnixClient)client).Connect());

await Task.Delay(1000);

server.Send(new Test_data(Guid.NewGuid(), "HEllo world"));

while (true) ;


public record struct Test_data(Guid Id, string Name) : IMessage;