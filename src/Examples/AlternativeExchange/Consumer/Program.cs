using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using ExchangeType = Core.Commons.Enums.ExchangeType;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Consumer";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var altExchangeName = ExampleData.GetCustomExchangeName("alternative");
RabbitMqHelper.CreateExchange(channel, altExchangeName, ExchangeType.FanOut);

var altQueue = ExampleData.GetCustomQueueName("alt-queue");
RabbitMqHelper.CreateQueue(channel, altQueue);
RabbitMqHelper.BindQueue(channel, altQueue, altExchangeName, routingKey: string.Empty); // Alt Exchange Queue routing key is deliberately set to empty (default = means queueName) as no exchange will be directly pushing messages to it, but this will be happening automatically by main exchange so to avoid conflicts between exchange connections did so.

var altConsumer = new EventingBasicConsumer(channel);
altConsumer.Received += (model, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"AltQueue - message: {message}");
};

RabbitMqHelper.BasicConsumeMessage(altConsumer, channel, altQueue, autoAck: true, logToConsole: false);


var mainExchangeName = ExampleData.GetCustomExchangeName("main");
RabbitMqHelper.CreateExchange(
    channel,
    mainExchangeName,
    ExchangeType.Direct,
    new Dictionary<string, object> { { "alternative-exchange", altExchangeName }
    });

var mainQueue = ExampleData.GetCustomQueueName("main-queue");
RabbitMqHelper.CreateQueue(channel, mainQueue);
channel.QueueBind(mainQueue, mainExchangeName, routingKey: ExampleData.GetCustomRoutingKeyName("main"));
var mainConsumer = new EventingBasicConsumer(channel);
mainConsumer.Received += (model, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"MainQueue - message: {message}");
};

RabbitMqHelper.BasicConsumeMessage(mainConsumer, channel, mainQueue, autoAck: true, logToConsole: false);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);