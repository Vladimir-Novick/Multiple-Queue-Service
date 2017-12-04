using System;
using System.Collections.Generic;

using System.Runtime.Serialization;

namespace MQS.NetCore2.Api
{
    [DataContract]
    public class StorageObject
    {
        [DataMember]
        public string ErrorMessage { get; set; }

        public StorageObject()
        {
            ErrorMessage = "";
            Key = "";
            ProducerID = "";
            Data = "";
        }

        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string ProducerID { get; set; }
        [DataMember]
        public String Data { get; set; }
        [DataMember]
        public QueueStatus Status { get; set; }
    }
}
