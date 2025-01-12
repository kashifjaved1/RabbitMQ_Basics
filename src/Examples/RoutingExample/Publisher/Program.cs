using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

//RabbitMqHelper.CreateExchange(channel, ExampleData.MyDirectExchange, ExchangeType.Direct);
RabbitMqHelper.CreateExchange(channel, ExampleData.MyTopicExchange, ExchangeType.Topic); // comment it for using direct exchange.

var message = "Message to broadcast to multiple consumers!";
//RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.MyDirectExchange, routingKey: ExampleData.GetCustomRoutingKeyName(1), message: message);
//RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.MyDirectExchange, routingKey: ExampleData.GetCustomRoutingKeyName(2), message: message);
//RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.MyDirectExchange, routingKey: ExampleData.CommonRoutingKey, message: message);

RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.MyTopicExchange, routingKey: "user.asia.payments", message: message); // comment it for to use above direct exchange publishing.

while (true)
{
    var endKey = Console.ReadKey();
    if (endKey.Key == ConsoleKey.Enter)
    {
        RabbitMqHelper.CloseConnection(channel, connection);
        break;
    }
}