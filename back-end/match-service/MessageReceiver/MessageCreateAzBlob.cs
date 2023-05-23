using Azure.Storage.Blobs.Models;
using System.Text;
using Azure.Storage.Blobs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using MongoDB.Driver;

public class MessageCreateAzBlob : IHostedService
{
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly ConnectionFactory factory;
    BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=tenderblob;AccountKey=h+mQabKquq6HhmW/CVKKnUG1l5iUjeiTEHys06y4wXqiyltbQz/Pph3hxHmGJRaxDYZ4rPeaVP/i+ASti3NO0A==;EndpointSuffix=core.windows.net");


    MongoClient dbClient = new MongoClient("mongodb+srv://neykneyk1:081100neyko@tender.55ndihf.mongodb.net/test");
    private IMongoDatabase database;
    private IMongoCollection<MatchModel> collection;

    public MessageCreateAzBlob()
    {
        this.database = dbClient.GetDatabase("MatchTender");
        this.collection = database.GetCollection<MatchModel>("Match");

        factory = new ConnectionFactory();
        factory.Uri = new Uri("amqp://localhost:5672");
        factory.ClientProvidedName = "Tender/BlobCreate";

        connection = factory.CreateConnection();
        channel = connection.CreateModel();

        channel.QueueDeclare("AzureBlobCreation",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

        cancellationTokenSource = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (sender, args) =>
        {
            //we manipulate the received messgae here and do what we want to (Save to DB etc.)

            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);

            User? user = JsonSerializer.Deserialize<User>(args.Body.ToArray());
            
            MatchModel insertEmptyMatch = new MatchModel{
                id = user.id,
                MatchesForUser = new List<User>()
            };
            await collection.InsertOneAsync(insertEmptyMatch);
            
            user.id = user.id.Replace("|","-");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(user.id);
            if(!await containerClient.ExistsAsync()){
                await containerClient.CreateAsync(PublicAccessType.BlobContainer);
            }

            Console.WriteLine($"Id: {message}");

            //once we acknowledge it, the message is gone
            channel.BasicAck(args.DeliveryTag, false);
        };
        channel.BasicConsume("AzureBlobCreation", false, consumer);

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
