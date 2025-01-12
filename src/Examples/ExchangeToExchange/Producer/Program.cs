using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

RabbitMqHelper.CreateExchange(channel, ExampleData.GetCustomExchangeName(1), ExchangeType.Direct);
RabbitMqHelper.CreateExchange(channel, ExampleData.GetCustomExchangeName(2), ExchangeType.FanOut);
RabbitMqHelper.BindExchangeToExchange(channel, ExampleData.GetCustomExchangeName(1), ExampleData.GetCustomExchangeName(2), ExampleData.GetCustomRoutingKeyName("e2e"));

var message = "This is a exchange-to-exchange message!";
RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.GetCustomExchangeName(1), routingKey: ExampleData.GetCustomRoutingKeyName("e2e"), message: message);

Console.ReadKey();
RabbitMqHelper.CloseConnection(channel, connection);