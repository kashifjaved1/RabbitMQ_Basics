using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

string exchangeName = string.Empty; // Default Exchange.
var useDefaultExchange = RabbitMqHelper.UseDefaultExchange();
if (!useDefaultExchange)
{
    exchangeName = ExampleData.MyExchange;
    RabbitMqHelper.CreateExchange(channel, exchangeName, ExchangeType.Direct);
}

RabbitMqHelper.CreateQueue(channel, ExampleData.MyQueue);
RabbitMqHelper.BindQueue(channel, ExampleData.MyQueue, exchangeName, ExampleData.MyRoutingKey);

var message = "This is a basic message!";
RabbitMqHelper.BasicPublishMessage(channel, exchange: exchangeName, routingKey:ExampleData.MyRoutingKey, message: message);
RabbitMqHelper.CloseConnection(channel, connection);