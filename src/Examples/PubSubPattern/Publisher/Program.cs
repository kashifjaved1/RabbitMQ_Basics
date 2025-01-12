using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

RabbitMqHelper.CreateExchange(channel, ExampleData.MyDirectExchange, ExchangeType.FanOut);

var message = "Broadcastable message!";
RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.MyDirectExchange, routingKey: ExampleData.MyCustomRoutingKey, message: message);
RabbitMqHelper.CloseConnection(channel, connection);