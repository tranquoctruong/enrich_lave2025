using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using User.API.Model;
using User.API.Services;

namespace User.API.Consumers
{
    public class ProductMessageConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ProductMessageConsumer> _logger;

        public ProductMessageConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<ProductMessageConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "product.created",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"[Product Consumer] Message: {message}");

                using var scope = _serviceScopeFactory.CreateScope();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                // Parse message and update DB
                var productDto = JsonSerializer.Deserialize<ProductCreatedMessage>(message);
                await userService.UpdateSomethingFromProduct(productDto);
            };

            await channel.BasicConsumeAsync(queue: "product.created",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }

}
