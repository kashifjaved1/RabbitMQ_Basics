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
        /// My custom queue
        /// </summary>
        public const string MyCustomQueue = "MY_CUSTOM_QUEUE";

        /// <summary>
        /// My custom routing key
        /// </summary>
        public const string MyCustomRoutingKey = "MY_CUSTOM_ROUTING_KEY";

        /// <summary>
        /// The common routing key
        /// </summary>
        public const string CommonRoutingKey = "COMMON_ROUTING_KEY";

        /// <summary>
        /// My dynamic custom routing key
        /// </summary>
        public const string MyDynamicCustomRoutingKey = "MY_DYNAMIC_CUSTOM_ROUTING_KEY{0}";

        /// <summary>
        /// My custom hash wildcard topic routing key
        /// </summary>
        public const string MyCustomHashWildcardTopicRoutingKey = "abc.#";

        /// <summary>
        /// My custom sterik wildcard topic routing key
        /// </summary>
        public const string MyCustomSterikWildcardTopicRoutingKey = "*.xyz.*";
    }                       
}                           
