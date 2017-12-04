using System;
using System.Runtime.Serialization;

namespace MQS.NetCore2.Server.Models
{
    [DataContract(Namespace = ""), Serializable]
    public class QueListItem
    {
         [DataMember]
        public DateTime Date_Create { get; set;  }  // DataC

        [DataMember]
        public String Key { get; set; }

        [DataMember]
        public int QueueCount { get; set; }

        [DataMember]
        public int JMyQueueCount { get; set; }
        [DataMember]
        public int Opened { get; internal set; }

        [DataMember]
        public int Total { get; internal set; }
    }
}
