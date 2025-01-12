using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var exchangeName = ExampleData.GetCustomExchangeName("with_headers");
RabbitMqHelper.CreateExchange(channel, exchangeName, ExchangeType.Headers);

var message = "This is a message with headers!";
var properties = channel.CreateBasicProperties();
properties.Headers = new Dictionary<string, object>
{
    { "name", "abc" }
};

RabbitMqHelper.BasicPublishMessage(channel, exchange: exchangeName, routingKey: string.Empty, message: message, properties: properties);

Console.ReadKey();
RabbitMqHelper.CloseConnection(channel, connection);