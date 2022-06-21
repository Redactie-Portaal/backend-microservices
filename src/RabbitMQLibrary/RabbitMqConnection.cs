using RabbitMQ.Client;

namespace RabbitMQLibrary
{
    /// <summary>
    /// Singleton service keeping track of our connection with RabbitMQ.
    /// </summary>
    public class RabbitMqConnection : IDisposable
    {
        private IConnection _connection;
        public IModel CreateChannel()
        {
            var connection = GetConnection();
            return connection.CreateModel();
        }

        private IConnection GetConnection()
        {
            if (_connection == null)
            {
                string? password = Environment.GetEnvironmentVariable("redactieportaal_rabbitmq_pass") ?? throw new ArgumentNullException("No Password Found");

                var factory = new ConnectionFactory
                {
                    UserName = "guest",
                    Password = password,
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                    AutomaticRecoveryEnabled = true
                };
                var endpoints = new List<AmqpTcpEndpoint>
                {
                          new AmqpTcpEndpoint("rabbitmq"),
                          new AmqpTcpEndpoint("localhost"),
                          new AmqpTcpEndpoint("production-rabbitmqcluster"),
                          new AmqpTcpEndpoint("production-rabbitmqcluster-server-0")
                };
                _connection = factory.CreateConnection(endpoints);
            }

            return _connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
