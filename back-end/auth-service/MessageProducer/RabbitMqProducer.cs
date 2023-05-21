using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

public class RabbitMqProducer : IRabbitMqProducer
{
    ConnectionFactory factory = new();

    public RabbitMqProducer()
    {
        factory.Uri = new Uri("amqp://rabbitmq:5672");
        factory.ClientProvidedName = "Tender/Auth-Service";
    }

    public void SendMessage<T>(T message, string routingKey,string queueName)
    {
        var connection = factory.CreateConnection();

        using (var channel = connection.CreateModel())
        {
            string exchangeName = "CreateAzureBlob/CreateMatch";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

            channel.QueueDeclare(queueName,
            durable: true,
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
