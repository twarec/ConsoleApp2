using ConsoleApp2.Models;

namespace ConsoleApp2.Domain;

public interface IClient : IObservable<IMessage>
{
}
