using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var exchangeName = ExampleData.GetCustomExchangeName("consistent_hashing");
RabbitMqHelper.CreateExchange(channel, exchangeName, ExchangeType.ConsistentHashing);

var message = "Hey! hash the routing and pass me plz.";

RabbitMqHelper.BasicPublishMessage(channel, exchange: exchangeName, routingKey: ExampleData.GetCustomRoutingKeyName("consistent_hashing"), message: message);

Console.ReadKey();
RabbitMqHelper.CloseConnection(channel, connection);