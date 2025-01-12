using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commons.Enums
{
    public enum ExchangeType
    {
        /// <summary>
        /// Exchange type used for AMQP direct exchanges.
        /// </summary>
        Direct,

        /// <summary>
        /// Exchange type used for AMQP fanout exchanges.
        /// </summary>
        FanOut,

        /// <summary>
        /// Exchange type used for AMQP headers exchanges.
        /// </summary>
        Headers,

        /// <summary>
        /// Exchange type used for AMQP topic exchanges.
        /// </summary>
        Topic,

        /// <summary>
        /// The consistent hashing
        /// </summary>
        ConsistentHashing
    }
}
