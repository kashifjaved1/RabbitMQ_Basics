using System.Text;
using Core.Commons.Exceptions;
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
        public static string BasicPublishMessage(IModel channel, string routingKey, string message, string? exchange = null, IBasicProperties? properties = null)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchange ?? string.Empty, routingKey: routingKey, basicProperties: properties, body: body);
                Console.WriteLine($"published message: {message}");

                return "Message(s) published";
            }
            catch (Exception ex)
            {
                return $"Publish failed: {ex.Message}";
            }
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
        public static string BasicConsumeMessage(EventingBasicConsumer consumer, IModel channel, string queue, bool autoAck = true)
        {
            try
            {
                consumer.Received += (msg, args) =>
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"published message: {message}");
                };

                channel.BasicConsume(queue, autoAck, consumer);

                return "Message(s) consumed.";
            }
            catch (Exception ex)
            {
                return $"Consume failed: {ex.Message}";
            }
        }
    }
}
