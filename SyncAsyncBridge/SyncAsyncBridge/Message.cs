using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncAsyncBridge.SyncAsyncBridge
{
    internal class Message<T>
    {
        public T Payload { get; private set; }
        public String MessageId { get; private set; }

        public Message(T payload, string messageId)
        {
            this.Payload = payload;
            this.MessageId = messageId;
        }

       
    }
}
