using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Consumer";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var exchagneName = ExampleData.GetCustomExchangeName(2);
RabbitMqHelper.CreateExchange(channel, exchagneName, ExchangeType.FanOut);

RabbitMqHelper.CreateQueue(channel, ExampleData.MyCustomQueue);
RabbitMqHelper.BindQueue(channel, ExampleData.MyCustomQueue, exchagneName, ExampleData.GetCustomRoutingKeyName("e2e"));

var consumer = RabbitMqHelper.GetConsumer(channel);
RabbitMqHelper.BasicConsumeMessage(consumer, channel, ExampleData.MyCustomQueue);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);