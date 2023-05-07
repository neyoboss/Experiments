public interface IRabbitMqProducer
{
    void SendMessage<T>(T message,string routingKey,string queueName);
}