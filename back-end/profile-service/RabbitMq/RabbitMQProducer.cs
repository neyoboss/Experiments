using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

public class RabbitMQProducer : IRabbitMQProducer
{
    public void SendMessage<T>(T message)
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://rabbit:5672");
        factory.ClientProvidedName = "Tender/Profile-Service";

        var connection = factory.CreateConnection();

        using (var channel = connection.CreateModel())
        {
            string exchangeName = "ProfileExchange";
            string routingKey = "profile";
            string queueName = "ProfileQueue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

            channel.QueueDeclare(queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

            channel.QueueBind(queueName, exchangeName, routingKey, null);

            var json = JsonConvert.SerializeObject(message);
            var messageBody = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchangeName, routingKey, null, messageBody);
        }
    }
}