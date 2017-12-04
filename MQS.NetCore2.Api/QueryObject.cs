using System.Runtime.Serialization;

namespace MQS.NetCore2.Api
{
    [DataContract]
    public class QueryObject
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public StorageObject DataObject { get; set; }
        [DataMember]
        public QueueStatus Status { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
