using RabbitMQ.Client;

public class RabbitMqTest
{
    public RabbitMqTest()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var conenction = factory.CreateConnection();
        using var channel = conenction.CreateModel();

        channel.QueueDeclare(queue: "test",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
}