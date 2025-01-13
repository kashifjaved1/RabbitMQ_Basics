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

var deadLetterExchangeName = "dlx";
RabbitMqHelper.CreateExchange(channel, deadLetterExchangeName, ExchangeType.FanOut);

var mainExchangeName = "mainx";
RabbitMqHelper.CreateExchange(
    channel,
    mainExchangeName,
    ExchangeType.Direct,
    new Dictionary<string, object>
    {
        { "x-dead-letter-exchange", deadLetterExchangeName },
        { "x-message-ttl", 1000 }
    });

var mainQueue = "mainx-queue";
RabbitMqHelper.CreateQueue(channel, mainQueue);
channel.QueueBind(mainQueue, mainExchangeName, routingKey: ExampleData.GetCustomRoutingKeyName("main"));
var mainConsumer = new EventingBasicConsumer(channel);
mainConsumer.Received += (model, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Main - message: {message}");
};

//Task.Delay(TimeSpan.FromSeconds(2000)).Wait(); // adding more wait time deliberately so that message(s) will expire on main queue and will lands to deadLetter exchange.
//channel.BasicConsume(consumer: mainConsumer, queue: mainQueue, autoAck: true);

var deadLetterQueue = "dlx-queue";
RabbitMqHelper.CreateQueue(channel, deadLetterQueue);
RabbitMqHelper.BindQueue(channel, deadLetterQueue, deadLetterExchangeName, string.Empty);
channel.QueueBind(deadLetterQueue, deadLetterExchangeName, routingKey: string.Empty);
var deadLetterConsumer = new EventingBasicConsumer(channel);
deadLetterConsumer.Received += (model, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"DLX - message: {message}");
};

channel.BasicConsume(consumer: deadLetterConsumer, queue: deadLetterQueue, autoAck: true);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);