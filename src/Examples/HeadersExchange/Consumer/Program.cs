using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "Consumer";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

var exchagneName = ExampleData.GetCustomExchangeName("with_headers");
RabbitMqHelper.CreateExchange(channel, exchagneName, ExchangeType.Headers);

var queueName = ExampleData.GetCustomQueueName(1);
RabbitMqHelper.CreateQueue(channel, queueName);

var bindingArgs = new Dictionary<string, object>
{
    //{ "x-match", "all" }, // means if all header properties of headerExchange matches with this queue then it'll receive the data.
    { "x-match", "any" }, // means if any header propertie of headerExchange matches with this queue then it'll receive the data.
    { "name", "abc" },
    { "rank", "xyz" }
};

RabbitMqHelper.BindQueue(channel, queueName, exchagneName, string.Empty, arguments: bindingArgs);

var consumer = RabbitMqHelper.GetConsumer(channel);
RabbitMqHelper.BasicConsumeMessage(consumer, channel, queueName);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);