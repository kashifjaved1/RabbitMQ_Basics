using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Consumer";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

string exchangeName = string.Empty; // Default Exchange.
var useDefaultExchange = RabbitMqHelper.UseDefaultExchange();
if (!useDefaultExchange)
{
    exchangeName = ExampleData.MyDirectExchange;
    RabbitMqHelper.CreateExchange(channel, exchangeName, ExchangeType.Direct); // Doing in consumer because just in case it starts before producer,
    // so we have to make sure that exchange exists before proceeding with anything next.
}

RabbitMqHelper.CreateQueue(channel, ExampleData.MyCustomQueue);
RabbitMqHelper.BindQueue(channel, ExampleData.MyCustomQueue, exchangeName, ExampleData.MyCustomRoutingKey);

var consumer = RabbitMqHelper.GetConsumer(channel);
RabbitMqHelper.BasicConsumeMessage(consumer, channel, ExampleData.MyCustomQueue);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);