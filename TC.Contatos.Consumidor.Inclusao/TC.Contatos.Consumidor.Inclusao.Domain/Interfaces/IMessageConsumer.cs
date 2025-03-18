using Domain.RegionalAggregate;

namespace Domain.Interfaces;

public interface IMessageConsumer
{
    event Func<Contato, Task>? OnMessageReceived;
    Task ConsumeAsync();
}

