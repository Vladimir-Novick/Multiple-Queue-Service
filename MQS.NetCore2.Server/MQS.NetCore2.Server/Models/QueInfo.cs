using System;
using System.Runtime.Serialization;

namespace MQS.NetCore2.Server.Models
{
    [DataContract(Namespace = ""), Serializable]
    public class QueInfo
    {

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public String Key { get; set; }

        [DataMember]
        public int Count { get; set; }

    }
}
