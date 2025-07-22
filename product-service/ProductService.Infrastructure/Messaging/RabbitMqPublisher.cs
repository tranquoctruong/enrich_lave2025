using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductService.Application.Interfaces;
using RabbitMQ.Client;

namespace ProductService.Infrastructure.Messaging
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"] ?? "localhost"
            };
        }

        public async Task PublishAsync<T>(T message, string exchange, string routeKey)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            // Khai báo Exchange kiểu Fanout
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            // Với Fanout, routingKey bị bỏ qua, nên để trống
            await channel.BasicPublishAsync(exchange, routingKey: "", mandatory: true, props, body);
        }

    }

}
