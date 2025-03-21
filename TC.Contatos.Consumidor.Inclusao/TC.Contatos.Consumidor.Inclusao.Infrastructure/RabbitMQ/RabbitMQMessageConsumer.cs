using Domain.RegionalAggregate;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Domain.Interfaces;

namespace Infrastructure.RabbitMQ;

public class RabbitMQMessageConsumer : IMessageConsumer
{
    private readonly Func<Task<IConnection>> _connectionFactory;
    private readonly IOptions<RabbitMQSettings> _settings;

    public event Func<Contato, Task>? OnMessageReceived;

    public RabbitMQMessageConsumer(Func<Task<IConnection>> connectionFactory, IOptions<RabbitMQSettings> settings)
    {
        _connectionFactory = connectionFactory;
        _settings = settings;
    }

    public async Task ConsumeAsync()
    {
        try
        {
            var connection = await _connectionFactory();

            using var channel = await connection.CreateChannelAsync();
            {

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (sender, eventArgs) =>
                {
                    try
                    {
                        var body = eventArgs.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var contato = JsonSerializer.Deserialize<Contato>(message);

                        if (OnMessageReceived != null)
                        {
                            await OnMessageReceived(contato);
                            await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                        }
                    }
                    catch (Exception exception)
                    {
                        await channel.BasicNackAsync(eventArgs.DeliveryTag, false, false);
                    }
                };

                await channel.BasicConsumeAsync(
                        queue: _settings.Value.Queue,
                        autoAck: false,
                        consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}

