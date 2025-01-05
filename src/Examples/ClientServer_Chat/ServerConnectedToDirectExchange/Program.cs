using Core.Commons;

var channel = RabbitMqDirectExchangeHelper.CreateRabbitMqChannel();

var message = "This is a message to publish.";
var routeKey = "";

RabbitMqDirectExchangeHelper.BasicPublishMessage(channel, routeKey, message);