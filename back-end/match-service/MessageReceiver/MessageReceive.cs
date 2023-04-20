using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
public class MessageReceive : IHostedService
{

    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string queueName = "ProfileQueue";

    public MessageReceive()
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqp://rabbitmq:5672");
        factory.ClientProvidedName = "Tender/Match-Service";

        connection = factory.CreateConnection();
        channel = connection.CreateModel();

        channel.QueueDeclare(queueName,
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

        cancellationTokenSource = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            //we manipulate the received messgae here and do what we want to (Save to DB etc.)

            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message Received: {message}");

            //once we acknowledge it, the message is gone
            channel.BasicAck(args.DeliveryTag, false);
            //string consumerTag = channel.BasicConsume(queueName, false, consumer);
        };
        channel.BasicConsume(queueName, false, consumer);

        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource.Cancel();
        channel.Close();
        connection.Close();

        return Task.CompletedTask;
    }
}