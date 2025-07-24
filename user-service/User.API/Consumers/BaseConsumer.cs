using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Logging;

namespace User.API.Consumers
{
    public static class BaseConsumer
    {
        public async static Task StartConsumerAsync(string exchange, string queueName,ILogger logger)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            // Delcare exchange type fanout
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout, durable: true);

            // Declare for exchange
            await channel.QueueDeclareAsync(queue: queueName,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false);

            // Bind queue to exchange fanout
            await channel.QueueBindAsync(queue: queueName,
                                         exchange: exchange,
                                         routingKey: ""); 

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                logger.LogInformation($"[{queueName}] Received: {message}");

                // logic handle...
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
