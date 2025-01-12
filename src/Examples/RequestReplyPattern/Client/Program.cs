using Core.Commons.Helpers;
using Core.Commons.Shared;
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Started Client!");

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Client";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var replyQueue = RabbitMqHelper.CreateQueue(channel, queueName: string.Empty, exclusive: true).QueueName;
var consumer = RabbitMqHelper.GetConsumer(channel);
consumer.Received += (model, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received reply: {message}");
};

channel.BasicConsume(queue: replyQueue, autoAck: true, consumer: consumer);

RabbitMqHelper.CreateQueue(channel, queueName: ExampleData.RequestQueue, exclusive: true);
var message = "can I request a reply?";
var properties = channel.CreateBasicProperties();
properties.ReplyTo = replyQueue; // This property is for publisher so that it can use it to send reply back.
properties.CorrelationId = Guid.NewGuid().ToString();

Console.WriteLine($"Sending request with id: {properties.CorrelationId} and message: {message}");
RabbitMqHelper.BasicPublishMessage(channel, routingKey: ExampleData.RequestQueue, message, properties: properties, logToConsole: false); // here using routingKey 'RequestQue' same as requestQueue name as that queue binded to exhange without bindingKey and in that case queueName becomes the bindingKey so it'll publish data to requestQueue.

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);