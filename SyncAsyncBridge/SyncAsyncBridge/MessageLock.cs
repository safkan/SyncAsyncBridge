using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncAsyncBridge.SyncAsyncBridge
{
    internal class MessageLock<TReceive>
    {
        public string JobId { get; set; }
        public TReceive MessageReceived { get; set; }
    }
}
