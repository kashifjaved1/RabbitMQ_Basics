﻿using Core.Commons.Enums;
using Core.Commons.Helpers;
using Core.Commons.Shared;

Uri uri = new Uri("amqp://guest:guest@localhost:5672");
string clientName = "User Consumer";

var connectionFactory = RabbitMqHelper.GetConnectionFactory(uri, clientName);
var connection = RabbitMqHelper.OpenConnection(connectionFactory);
var channel = RabbitMqHelper.CreateChannel(connectionFactory, connection);

RabbitMqHelper.CreateExchange(channel, ExampleData.MyTopicExchange, ExchangeType.Topic);

var queueName = RabbitMqHelper.CreateTemporaryQueue(channel);
RabbitMqHelper.BindQueue(channel, queueName, ExampleData.MyTopicExchange, "user.#");
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = RabbitMqHelper.GetConsumer(channel);
RabbitMqHelper.BasicConsumeMessage(consumer, channel, queueName, autoAck: true, clientName: clientName);

Console.ReadLine();
RabbitMqHelper.CloseConnection(channel, connection);