using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var altExchangeName = "alternate"; // ExampleData.GetCustomExchangeName("main");
RabbitMqHelper.CreateExchange(channel, altExchangeName, ExchangeType.FanOut);

var mainExchangeName = "main"; // ExampleData.GetCustomExchangeName("main");
RabbitMqHelper.CreateExchange(
    channel,
    mainExchangeName,
    ExchangeType.Direct,
    new Dictionary<string, object> { { "alternate-exchange", "alternate" } 
    });

var message = "This is a alternative-exchange pattern message!";
RabbitMqHelper.BasicPublishMessage(channel, exchange: mainExchangeName, routingKey: ExampleData.GetCustomRoutingKeyName("main"), message: message);
RabbitMqHelper.BasicPublishMessage(channel, exchange: mainExchangeName, routingKey: "abc", message: message); // As this routingKey isn't used to queue to get binded with exchagne so this published message will landing messages to altExchange.

Console.ReadKey();
RabbitMqHelper.CloseConnection(channel, connection);