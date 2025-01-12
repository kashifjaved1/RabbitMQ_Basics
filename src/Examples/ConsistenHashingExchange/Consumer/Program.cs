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

var exchagneName = ExampleData.GetCustomExchangeName("consistent_hashing");
RabbitMqHelper.CreateExchange(channel, exchagneName, ExchangeType.ConsistentHashing);

var queue1 = ExampleData.GetCustomQueueName(1);
var queue2 = ExampleData.GetCustomQueueName(2);

RabbitMqHelper.CreateQueue(channel, queue1);
RabbitMqHelper.CreateQueue(channel, queue2);

channel.QueueBind(queue1, exchagneName, "1"); // This 1 is basically 1/4, means it'll consume binded exchange 25%.
channel.QueueBind(queue2, exchagneName, "3"); // This 3 is basically 3/4, means it'll consume binded exchange 75%.

var consumer1 = new EventingBasicConsumer(channel);
consumer1.Received += (model, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue1 - message: {message}");
};

RabbitMqHelper.BasicConsumeMessage(consumer1, channel, queue1, logToConsole: false);

var consumer2 = new EventingBasicConsumer(channel);
consumer2.Received += (model, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue2 - message: {message}");
};

RabbitMqHelper.BasicConsumeMessage(consumer2, channel, queue2, logToConsole: false);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);