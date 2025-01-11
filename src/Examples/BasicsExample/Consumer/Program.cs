using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Consumer";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

RabbitMqHelper.CreateExchange(channel, ExampleData.MyExchange, ExchangeType.Direct);
RabbitMqHelper.CreateQueue(channel, ExampleData.MyQueue);
RabbitMqHelper.BindQueue(channel, ExampleData.MyQueue, ExampleData.MyExchange, ExampleData.MyRoutingKey);

var consumer = RabbitMqHelper.GetConsumer(channel);
RabbitMqHelper.BasicConsumeMessage(consumer, channel, ExampleData.MyQueue);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);