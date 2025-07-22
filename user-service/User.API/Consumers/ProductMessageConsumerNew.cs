using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using User.API.Model;
using User.API.Services;

namespace User.API.Consumers
{
    public class ProductMessageConsumer2 : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ProductMessageConsumer> _logger;

        public ProductMessageConsumer2(IServiceScopeFactory serviceScopeFactory, ILogger<ProductMessageConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await BaseConsumer.StartConsumerAsync("product.fanout", "consumerNew.queue", _logger);
        }
    }

}
