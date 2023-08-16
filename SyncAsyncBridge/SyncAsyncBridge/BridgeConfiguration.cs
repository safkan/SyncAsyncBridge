using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncAsyncBridge.SyncAsyncBridge
{
    public class BridgeConfiguration<TSend>
    {
        /// <summary>
        /// This is the timeout to wait until the send/receive operation gives up.
        /// The value is in milliseconds.
        /// </summary>
        public int SendAndReceiveMessageTimeout { get; set; }
        
        /// <summary>
        /// The jobId which is used to identify jobs is incremental. However, its
        /// format is [prefix].[number], and it is a string. The prefix is useful
        /// in case of possible jobId collisions, and should be unique to the 
        /// instance. This should not be null, but can be set to the empty string.
        /// </summary>
        public string JobIdPrefix { get; set; }
        /// <summary>
        /// This is the action that the Bridge will use to send outgoing messages.
        /// This is usually a lambda function that will take the argument and send
        /// it to the proper async message thing.
        /// </summary>
        
        public Action<TSend> MessageSendingAction { get; set; }

    }
}
