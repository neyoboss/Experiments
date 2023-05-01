using Azure.Storage.Blobs.Models;
using System.Text;
using Azure.Storage.Blobs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class MessageCreateAzBlob : IHostedService
{
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string queueName = "AzureBlobCreation";
    BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=tenderblob;AccountKey=h+mQabKquq6HhmW/CVKKnUG1l5iUjeiTEHys06y4wXqiyltbQz/Pph3hxHmGJRaxDYZ4rPeaVP/i+ASti3NO0A==;EndpointSuffix=core.windows.net");

    public MessageCreateAzBlob()
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqp://rabbitmq:5672");
        factory.ClientProvidedName = "Tender/BlobCreate";

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
        consumer.Received += async (sender, args) =>
        {
            //we manipulate the received messgae here and do what we want to (Save to DB etc.)

            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            
            Console.WriteLine($"Message Received: {message}");

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(message);
            if(!await containerClient.ExistsAsync()){
                await containerClient.CreateAsync(PublicAccessType.BlobContainer);
            }

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