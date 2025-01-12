using Core.Commons.Exceptions;

namespace Core.Commons.Shared
{
    public static class ExampleData
    {
        /// <summary>
        /// My direct exchange
        /// </summary>
        public const string MyDirectExchange = "MY_DIRECT_EXCHANGE";

        /// <summary>
        /// My topic exchange
        /// </summary>
        public const string MyTopicExchange = "MY_TOPIC_EXCHANGE";

        /// <summary>
        /// My custom exchange name
        /// </summary>
        public const string MyCustomExchange = "MY_CUSTOM_EXCHANGE";

        /// <summary>
        /// My custom queue
        /// </summary>
        public const string MyCustomQueue = "MY_CUSTOM_QUEUE";

        /// <summary>
        /// The request queue
        /// </summary>
        public const string RequestQueue = "REQUEST_QUEUE";

        /// <summary>
        /// My custom routing key
        /// </summary>
        public const string MyCustomRoutingKey = "MY_CUSTOM_ROUTING_KEY";

        /// <summary>
        /// The common routing key
        /// </summary>
        public const string CommonRoutingKey = "COMMON_ROUTING_KEY";

        /// <summary>
        /// Gets the name of the custom routing.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The custom routing key name.</returns>
        public static string GetCustomRoutingKeyName(object value)
        {
            return GetCustomName(MyCustomRoutingKey, value);
        }

        /// <summary>
        /// Gets the name of the custom queue.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The custom queue name</returns>
        public static string GetCustomQueueName(object value)
        {
            return GetCustomName(MyCustomQueue, value);
        }

        /// <summary>
        /// Gets the wildcard routing key.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>The wildcard type routing key</returns>
        /// <exception cref="Core.Commons.Exceptions.InvalidWildcardTypeException"></exception>
        public static string GetWildcardRoutingKey(char wildCardType)
        {
            switch (wildCardType)
            {
                case '*':
                    return "*.xyz.*";

                case '#':
                    return "abc.#";

                default:
                    throw new InvalidWildcardTypeException();
            }
        }

        /// <summary>
        /// Gets the name of the custom exchange.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The custom routing key name.</returns>
        public static string GetCustomExchangeName(object value)
        {
            return GetCustomName(MyCustomExchange, value);
        }

        /// <summary>
        /// Gets the custom name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The custom routing key name.</returns>
        public static string GetCustomName(string value1, object value2)
        {
            return $"{value1}_{value2}";
        }
    }                       
}                           
