namespace RabbitMQLibrary.Producer
{
    /// <summary>
    /// Publishes a message on a message broker
    /// </summary>
    public interface IMessageProducer
    {
        /// <summary>
        /// Publishes a message to the message broker
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        void PublishMessageAsync<T>(string routingKey, T message);
    }
}
