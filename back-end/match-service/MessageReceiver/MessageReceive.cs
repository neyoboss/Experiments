using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class MessageReceive:IMessageReceive
{
    public void ReceiveMessage()
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
        factory.ClientProvidedName = "Tender/Match-Service";

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();


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
        channel.BasicQos(0, 1, false); //Set the size of the message, how many messages we want at one time and whether we want to apply it to the whole system or just this instance

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            //we manipulate the received messgae here and do what we want to (Save to DB etc.)

            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message Received: {message}");

            //once we acknowledge it, the message is gone
            channel.BasicAck(args.DeliveryTag, false);
        };

        string consumerTag = channel.BasicConsume(queueName, false, consumer);
        channel.BasicCancel(consumerTag);
    }
}