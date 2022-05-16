namespace NewsItemService.Interfaces
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message, string routingKey);
    }
}
