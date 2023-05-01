using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

public class RabbitMqProducer:IRabbitMqProducer
{
    public void SendMessage<T>(T message)
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://rabbitmq:5672");
        factory.ClientProvidedName = "Tender/Auth-Service";

        var connection = factory.CreateConnection();

        using (var channel = connection.CreateModel())
        {
            string exchangeName = "CreateAzureBlob";
            string routingKey = "blob";
            string queueName = "AzureBlobCreation";

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
