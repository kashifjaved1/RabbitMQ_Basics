using Core.Commons.Helpers;
using Core.Commons.Shared;
using System.Text;

Console.WriteLine("Started Server!");

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Server";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

RabbitMqHelper.CreateQueue(channel, queueName: ExampleData.RequestQueue, exclusive: false);
var consumer = RabbitMqHelper.GetConsumer(channel);
consumer.Received += (model, args) =>
{
    var receivedMessage = Encoding.UTF8.GetString(args.Body.ToArray());
    Console.WriteLine($"Request received with id: {args.BasicProperties.CorrelationId} and message: {receivedMessage}");

    var reply = $"This is a reply for requestId: {args.BasicProperties.CorrelationId}";

    RabbitMqHelper.BasicPublishMessage(channel, args.BasicProperties.ReplyTo, reply, logToConsole: false);
};

RabbitMqHelper.BasicConsumeMessage(consumer, channel, ExampleData.RequestQueue, autoAck: true, logToConsole: false);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);