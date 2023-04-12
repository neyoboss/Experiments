using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
public class MessageReceive : IHostedService
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();



    public MessageReceive(IServiceScopeFactory serviceScopeFactory)
    {
        this.serviceScopeFactory = serviceScopeFactory;
    }

    public void ReceiveMessage()
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var _matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();

            ConnectionFactory factory = new();
            factory.Uri = new Uri("amqp://rabbit:5672");
            factory.ClientProvidedName = "Tender/Match-Service/DeleteMatches";

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

                _matchService.DeleteMatches(message);
                //once we acknowledge it, the message is gone
                channel.BasicAck(args.DeliveryTag, false);
            };

            string consumerTag = channel.BasicConsume(queueName, false, consumer);
            channel.BasicCancel(consumerTag);
        }

    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ReceiveMessage();
                Task.Delay(TimeSpan.FromSeconds(2)).Wait();
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}