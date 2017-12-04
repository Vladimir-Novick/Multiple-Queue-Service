using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MQS.NetCore2.Server.Models
{

    [DataContract(Namespace = ""), Serializable]
    public class SuppliersItem
    {
        [DataMember]
        public string Supplier { get; set; }
        [DataMember]
        public int Status { get; set; }
    }
}
