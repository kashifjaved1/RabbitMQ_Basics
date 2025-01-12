using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "First Consumer";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

RabbitMqHelper.CreateExchange(channel, ExampleData.MyDirectExchange, ExchangeType.FanOut);

var queueName = RabbitMqHelper.CreateTemporaryQueue(channel);
RabbitMqHelper.BindQueue(channel, queueName, ExampleData.MyDirectExchange, ExampleData.MyCustomRoutingKey);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); 

var consumer = RabbitMqHelper.GetConsumer(channel);
RabbitMqHelper.BasicConsumeMessage(consumer, channel, queueName, clientName: clientName);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);