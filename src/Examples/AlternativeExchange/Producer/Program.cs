using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var altExchangeName = ExampleData.GetCustomExchangeName("alternative");
RabbitMqHelper.CreateExchange(channel, altExchangeName, ExchangeType.FanOut);

RabbitMqHelper.CreateExchange(
    channel,
    ExampleData.GetCustomExchangeName("main"),
    ExchangeType.Direct,
    new Dictionary<string, object> { { "alternative-exchange", altExchangeName } 
    });

var message = "This is a alternative-exchange pattern message!";
//RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.GetCustomExchangeName("main"), routingKey: ExampleData.GetCustomRoutingKeyName("main"), message: message);
RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.GetCustomExchangeName("main"), routingKey: "abc", message: message); // This is for landing messages to altExchange as no exchange having 'abc' routing key so its unrouteable and will land to altExchange.

Console.ReadKey();
RabbitMqHelper.CloseConnection(channel, connection);