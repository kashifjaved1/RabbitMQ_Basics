using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Analytics Consumer";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

//RabbitMqHelper.CreateExchange(channel, ExampleData.MyDirectExchange, ExchangeType.Direct);
RabbitMqHelper.CreateExchange(channel, ExampleData.MyTopicExchange, ExchangeType.Topic); // comment it for using direct exchange.

var queueName = RabbitMqHelper.CreateTemporaryQueue(channel);

//// [NOTE]: A Queue can have more than one binding at the same time and you can test this by uncommenting below queue bindings.
//RabbitMqHelper.BindQueue(channel, queueName, ExampleData.MyDirectExchange, RabbitMqHelper.GetCustomRoutingKeyName(1));
//RabbitMqHelper.BindQueue(channel, queueName, ExampleData.MyDirectExchange, ExampleData.CommonRoutingKey);

RabbitMqHelper.BindQueue(channel, queueName, ExampleData.MyTopicExchange, "*.asia.*"); // comment it for to use above direct exchange publishing.
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); 

var consumer = RabbitMqHelper.GetConsumer(channel);
RabbitMqHelper.BasicConsumeMessage(consumer, channel, queueName, autoAck: true, clientName: clientName);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);