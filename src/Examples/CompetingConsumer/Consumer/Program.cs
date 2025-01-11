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
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); // CompetingConsumer patter: It allows to process one message at a time so other consumers won't
                                                                    // get stopped for this consumer if it taking more than required time to process a message.

var consumer = RabbitMqHelper.GetConsumer(channel);
RabbitMqHelper.BasicConsumeMessage(consumer, channel, ExampleData.MyQueue);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);