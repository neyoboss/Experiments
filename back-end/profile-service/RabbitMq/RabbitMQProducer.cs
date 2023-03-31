using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

public class RabbitMQProducer : IRabbitMQProducer
{
    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        var connection = factory.CreateConnection();

        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare("profile",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var messageBody = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "profile", body: messageBody);
        }
    }
}