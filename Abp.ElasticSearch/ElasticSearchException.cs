using System;
using System.Runtime.Serialization;

namespace Abp.ElasticSearch
{
    /// <summary>
    /// Es异常
    /// </summary>
    [Serializable]
    public class ElasticSearchException : AbpException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ElasticSearchException()
        {

        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public ElasticSearchException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public ElasticSearchException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ElasticSearchException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
