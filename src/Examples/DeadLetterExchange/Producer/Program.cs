using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var mainExchangeName = "mainExchange";
RabbitMqHelper.CreateExchange(
    channel,
    mainExchangeName,
    ExchangeType.Direct);

var message = "This message need to expire so that it'll land to dead-letter exchange!";
RabbitMqHelper.BasicPublishMessage(channel, exchange: mainExchangeName, routingKey: ExampleData.GetCustomRoutingKeyName("main"), message: message);

Console.ReadKey();
RabbitMqHelper.CloseConnection(channel, connection);