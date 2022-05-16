using System.Text;
using NewsItemService.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace NewsItemService.Messaging
{
    class RabbitMQProducer : IMessageProducer
    {
        private readonly ILogger<RabbitMQProducer> _logger;

        public RabbitMQProducer(ILogger<RabbitMQProducer> logger)
        {
            _logger = logger;
        }

        public void SendMessage<T>(T message, string routingKey)
        {
            _logger.LogInformation($"Sending message {message} with routing key {routingKey}");

            string? password = Environment.GetEnvironmentVariable("redactieportaal_rabbitmq_pass") ?? throw new ArgumentNullException("No Password Found");

            ConnectionFactory factory = new ConnectionFactory() { 
                HostName = "localhost",
                UserName = "developer",
                Password = password,
            };

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "user-exchange", 
                                        type: ExchangeType.Topic);

                string messageBody = JsonConvert.SerializeObject(message);
                byte[] body = Encoding.UTF8.GetBytes(messageBody);

                channel.BasicPublish(exchange: "user-exchange",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}