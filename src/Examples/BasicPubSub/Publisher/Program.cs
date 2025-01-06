using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;
using Newtonsoft.Json;
using System.Text;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Publisher";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

RabbitMqHelper.CreateExchange(channel, ExampleData.MyExchange, ExchangeType.Direct);
RabbitMqHelper.CreateQueue(channel, ExampleData.MyQueue);
RabbitMqHelper.BindQueue(channel, ExampleData.MyQueue, ExampleData.MyExchange, ExampleData.MyRoutingKey);

var message = new
{
    ProductId = Guid.NewGuid(),
    ProductName = "ABC",
    Price = "100$"
};

var messageJson = JsonConvert.SerializeObject(message);
var messageBody = Encoding.UTF8.GetBytes(messageJson);

RabbitMqHelper.BasicPublishMessage(channel, exchange: ExampleData.MyExchange, routingKey:ExampleData.MyRoutingKey, messageBody: messageBody);
RabbitMqHelper.CloseConnection(channel, connection);