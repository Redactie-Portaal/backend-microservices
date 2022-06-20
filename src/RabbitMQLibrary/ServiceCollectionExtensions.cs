using Microsoft.Extensions.DependencyInjection;
using RabbitMQLibrary.Producer;

namespace RabbitMQLibrary
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessageProducing(this IServiceCollection services, string exchangeName)
        {
            // Add to every service in the microservices the queuename which is unique to each service
            var exchangeNameService = new ExchangeName(exchangeName);
            services.AddSingleton(exchangeNameService);
            // Create a new rabbitmq connection and add it to the service collection that way every service has the connection
            var connection = new RabbitMqConnection();
            services.AddSingleton(connection);
            // Give the IMessagePublisher its MessageProducer that way it can start producing messages

            services.AddSingleton<IMessageProducer, MessageProducer>();
        }
    }
}
