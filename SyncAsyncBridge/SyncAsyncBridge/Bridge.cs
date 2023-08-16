using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncAsyncBridge.SyncAsyncBridge
{
    public class Bridge<TSend, TReceive>
    {
     
   

        private ConcurrentDictionary<string, MessageLock<TReceive>> _jobIdToMessageLockDictionary { get; set; }

        private BridgeConfiguration<TSend> _bridgeConfiguration;

        private int _jobIdCounter;

        public Bridge(BridgeConfiguration<TSend> bridgeConfiguration)
        {
            _jobIdToMessageLockDictionary = new ConcurrentDictionary<string, MessageLock<TReceive>>();
            _bridgeConfiguration = bridgeConfiguration;
            _jobIdCounter = 0;
        }


        private int GetNextJobId()
        {
            return Interlocked.Increment(ref _jobIdCounter);
        }

        public TReceive SendAndReceiveMessage(TSend messageToSend)
        {
            int timeoutInMilliseconds = _bridgeConfiguration.SendAndReceiveMessageTimeout;
            string jobId = $"{_bridgeConfiguration.JobIdPrefix}.{GetNextJobId()}";


            MessageLock<TReceive> messageLock = new MessageLock<TReceive>
            {
                JobId = jobId,
            };

            _jobIdToMessageLockDictionary[jobId] = messageLock;

            _bridgeConfiguration.MessageSendingAction(messageToSend);

            lock (messageLock)
            {
                // This is where the waiting happens.
                Monitor.Wait(messageLock, timeoutInMilliseconds);
            }

            _jobIdToMessageLockDictionary.TryRemove(jobId, out messageLock);

            TReceive messageReceived = messageLock.MessageReceived;

            if (messageReceived == null)
            {
                throw new TimeoutException($"After having awaited for {timeoutInMilliseconds} milliseconds, there was no response to the message with JobId {jobId}");
            }

            return messageReceived;
        }

        public bool MessageReceived(TReceive messageReceived, string jobId)
        {
            if (_jobIdToMessageLockDictionary.TryGetValue(jobId, out MessageLock<TReceive> messageLock))
            {
                messageLock.MessageReceived = messageReceived;

                lock (messageLock)
                {
                    Monitor.Pulse(messageLock);
                }

                return true;
            }
            else
            {
                return false;
            }


        }

    }
}
