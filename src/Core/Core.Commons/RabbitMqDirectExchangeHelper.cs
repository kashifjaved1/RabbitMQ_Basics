using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Core.Commons
{
    /// <summary>
    /// The rabbit mq basic helper
    /// </summary>
    public static class RabbitMqDirectExchangeHelper
    {
        /// <summary>
        /// Gets the connection factory.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <returns>The connection factory</returns>
        public static ConnectionFactory GetConnectionFactory(string hostname = null)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();

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
        /// Creates the rabbit mq channel.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <returns>The rabbitMq channel IModel</returns>
        public static IModel CreateRabbitMqChannel(string hostname = null)
        {
            var connectionFactory = GetConnectionFactory(hostname);
            var connection = connectionFactory.CreateConnection();

            var channel = connection.CreateModel();
            return channel;
        }

        /// <summary>
        /// Basics the publish message.
        /// </summary>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routeKey">The route key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="message">The message.</param>
        /// <returns>The string response</returns>
        public static string BasicPublishMessage(IModel channel, string routeKey, string message, string exchange = null, IBasicProperties properties = null)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange, routeKey, properties, body);
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
        public static string ConsumeMessage(EventingBasicConsumer consumer, IModel channel, string queue, bool autoAck = true)
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
