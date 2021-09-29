using System;
using System.Runtime.Serialization;

namespace WhatsAppWeb
{
    [Serializable]
    internal class ContatoSendException : Exception
    {
        private Exception ex;

        public ContatoSendException()
        {
        }

        public ContatoSendException(Exception ex)
        {
            this.ex = ex;
        }

        public ContatoSendException(string message) : base(message)
        {
        }

        public ContatoSendException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContatoSendException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}