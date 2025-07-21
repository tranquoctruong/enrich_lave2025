namespace ProductService.Application.Interfaces
{
    public interface IRabbitMQPublisher
    {
        Task PublishAsync<T>(T message, string exchange, string routingKey);
    }
}
