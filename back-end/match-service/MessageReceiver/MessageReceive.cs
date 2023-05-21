using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class MessageReceive : IHostedService
{
    MongoClient dbClient = new MongoClient("mongodb+srv://neykneyk1:081100neyko@tender.55ndihf.mongodb.net/test");
    private IMongoDatabase database;
    private IMongoCollection<MatchModel> collection;

    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string queueName = "ProfileQueue";

    //private IMatchService matchService;

    public MessageReceive()
    {
        this.database = dbClient.GetDatabase("MatchTender");
        this.collection = database.GetCollection<MatchModel>("Match");

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

            User? user = JsonSerializer.Deserialize<User>(args.Body.ToArray());

            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);


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
