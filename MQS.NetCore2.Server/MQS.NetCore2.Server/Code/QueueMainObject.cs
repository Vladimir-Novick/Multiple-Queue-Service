using MQS.NetCore2.Api;
using System;
using System.Collections.Concurrent;
using System.Runtime.Serialization;

namespace MQS.NetCore2.Code
{
    [DataContract]
    public class QueueMainObject
    {
        public QueueMainObject()
        {
            Queue = new ConcurrentQueue<StorageObject>();
            DateC = DateTime.Now;
        }

        [DataMember]
        public DateTime DateC { get; private set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public ConcurrentQueue<StorageObject> Queue { get; set; }
         }
}
