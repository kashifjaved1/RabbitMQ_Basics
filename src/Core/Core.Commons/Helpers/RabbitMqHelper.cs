using System.ComponentModel.DataAnnotations;
using System.Text;
using Core.Commons.Exceptions;
using Core.Commons.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ExchangeType = Core.Commons.Enums.ExchangeType;

namespace Core.Commons.Helpers
{
    /// <summary>
    /// The rabbit mq basic helper
    /// </summary>
    public static class RabbitMqHelper
    {
        /// <summary>
        /// The random instance.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Gets the connection factory.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="clientName">Name of the client.</param>
        /// <returns>The connection factory.</returns>
        public static ConnectionFactory GetConnectionFactory(Uri uri, string? clientName = null)
        {
            ConnectionFactory connectionFactory = new()
            {
                Uri = uri,
                ClientProvidedName = clientName
            };

            return connectionFactory;
        }

        /// <summary>
        /// Gets the connection factory.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <returns>The connection factory.</returns>
        public static ConnectionFactory GetConnectionFactory(string? hostname = null)
        {
            ConnectionFactory connectionFactory = new();

            if (string.IsNullOrEmpty(hostname))
            {
                connectionFactory.HostName = "localhost";
            }
            else
            {
                connectionFactory.HostName = hostname;
            }

            return connectionFactory;
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <returns>The rabbitMQ connection.</returns>
        public static IConnection OpenConnection(ConnectionFactory connectionFactory)
        {
            return connectionFactory.CreateConnection();
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="connection">The connection.</param>
        public static void CloseConnection(IModel channel, IConnection connection)
        {
            channel.Close();
            connection.Close();
        }

        /// <summary>
        /// Creates the exchange.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        /// <exception cref="Core.Commons.Exceptions.InvalidExchangeTypeException"></exception>
        public static void CreateExchange(IModel channel, string exchangeName, ExchangeType exchangeType)
        {
            switch (exchangeType)
            {
                case ExchangeType.Direct:
                    channel.ExchangeDeclare(exchangeName, RabbitMQ.Client.ExchangeType.Direct);
                    break;

                case ExchangeType.FanOut:
                    channel.ExchangeDeclare(exchangeName, RabbitMQ.Client.ExchangeType.Fanout);
                    break;

                case ExchangeType.Topic:
                    channel.ExchangeDeclare(exchangeName, RabbitMQ.Client.ExchangeType.Topic);
                    break;

                case ExchangeType.Headers:
                    channel.ExchangeDeclare(exchangeName, RabbitMQ.Client.ExchangeType.Headers);
                    break;

                default:
                    throw new InvalidExchangeTypeException();
            }
        }

        /// <summary>
        /// Creates the queue.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="durable">if set to <c>true</c> [durable].</param>
        /// <param name="exclusive">if set to <c>true</c> [exclusive].</param>
        /// <param name="autoDelete">if set to <c>true</c> [automatic delete].</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The Queue Declaration.</returns>
        public static QueueDeclareOk CreateQueue(IModel channel, string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object>? arguments = null)
        {
            return channel.QueueDeclare(queueName, false, false, false, null);
        }

        /// <summary>
        /// Creates the temporary queue.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The temp queueName.</returns>
        public static string CreateTemporaryQueue(IModel channel)
        {
            return channel.QueueDeclare().QueueName;
        }

        /// <summary>
        /// Binds the queue.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="arguments">The arguments.</param>
        public static void BindQueue(IModel channel, string queueName, string exchangeName, string routingKey, IDictionary<string, object>? arguments = null)
        {
            channel.QueueBind(queueName, exchangeName, routingKey, null);
        }

        /// <summary>
        /// Creates the channel.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>The rabbitMq channel.</returns>
        public static IModel CreateChannel(ConnectionFactory connectionFactory, IConnection connection)
        {
            OpenConnection(connectionFactory);
            var channel = connection.CreateModel();
            return channel;
        }

        /// <summary>
        /// Basic message publishing.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="routingKey">The route key.</param>
        /// <param name="message">The message.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="properties">The properties.</param>
        public static void BasicPublishMessage(IModel channel, string routingKey, string message, string? exchange = null, IBasicProperties? properties = null, bool logToConsole = true)
        {
            // Uncomment below code snippet for BasicsExample, PubSubPattern, RoutingExample.
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchange ?? string.Empty, routingKey: routingKey, basicProperties: properties, body: body);
                if (logToConsole)
                {
                    Console.WriteLine($"published message: {message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Publish failed: {ex.Message}");
            }

            //// Uncomment below code snippet for CompetingConsumer.
            //try
            //{
            //    var messageNo = 0;
            //    int publishingTime = 0;
            //    Console.WriteLine($"Publishing...");
            //    //Console.WriteLine($"Sending message #{messageNo++}");
            //    while (true)
            //    {
            //        publishingTime = Random.Next(1, 3);

            //        var body = Encoding.UTF8.GetBytes(message);
            //        channel.BasicPublish(exchange: exchange ?? string.Empty, routingKey: routingKey, basicProperties: properties, body: body);

            //        Task.Delay(TimeSpan.FromSeconds(publishingTime)).Wait();
            //        Console.WriteLine($"Message #{++messageNo} '{message}' has been published in total {publishingTime}s.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception($"Publish failed: {ex.Message}");
            //}
        }

        /// <summary>
        /// Gets the consumer.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns></returns>
        public static EventingBasicConsumer GetConsumer(IModel channel)
        {
            return new EventingBasicConsumer(channel);
        }

        /// <summary>
        /// Consumes the message.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="autoAck">if set to <c>true</c> [automatic ack].</param>
        /// <returns>The string consumed message</returns>
        public static void BasicConsumeMessage(EventingBasicConsumer consumer, IModel channel, string queue, bool autoAck = false, string? clientName = null, bool logToConsole = true) // setting autoAck to false by default
                                                                                                                                   // so that we'll manually acknowledge the message(s).
        {
            //// Uncomment below code snippet for BasicsExample, PubSubPattern, RoutingExample.
            try
            {
                string displayText = string.Empty;
                consumer.Received += (model, args) =>
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    if (string.IsNullOrEmpty(clientName))
                    {
                        displayText = $"Received message: '{message}'.";
                    }
                    else
                    {
                        displayText = $"{clientName} - Received message: '{message}'.";
                    }

                    if (logToConsole)
                    {
                        Console.WriteLine(displayText);
                    }
                };

                channel.BasicConsume(queue, autoAck, consumer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Consume failed: {ex.Message}");
            }

            // Uncomment below code snippet for CompetingConsumer.
            //try
            //{
            //    int publishedTime = 0;
            //    int messageNo = 0;
            //    string displayText = string.Empty;
            //    consumer.Received += (model, args) =>
            //    {
            //        publishedTime = Random.Next(1, 5);

            //        var body = args.Body.ToArray();
            //        var message = Encoding.UTF8.GetString(body);
            //        if (string.IsNullOrEmpty(clientName))
            //        {
            //            displayText = $"Received message #{++messageNo} '{message}', it'll take {publishedTime}s to process.";
            //        }
            //        else
            //        {
            //            displayText = $"{clientName} - Received message #{++messageNo} '{message}', it'll take {publishedTime}s to process.";
            //        }

            //        Console.WriteLine(displayText);

            //        Task.Delay(TimeSpan.FromSeconds(publishedTime)).Wait();
            //        channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false); // 'deliveryTag' is for ensuring that the message we're processing and the one one
            //        // acknowledge is same, and 'multiple: false' ensures that we only process one message at a time.
            //    };

            //    channel.BasicConsume(queue, autoAck, consumer);
            //    Console.WriteLine("Consuming...");
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception($"Consume failed: {ex.Message}");
            //}
        }

        /// <summary>
        /// Uses the default exchange.
        /// </summary>
        /// <returns>True or False regarding default exchange.</returns>
        public static bool UseDefaultExchange()
        {
            Console.Write("Want to use default? Y/N: ");
            
            while (true)
            {
                var choice = Console.ReadKey();
                if (choice.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (choice.Key == ConsoleKey.N)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the name of the custom routing.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The custom routing key name.</returns>
        public static string GetCustomRoutingKeyName(object obj)
        {
            return string.Format(nameof(ExampleData.MyDynamicCustomRoutingKey), obj);
        }
    }
}
