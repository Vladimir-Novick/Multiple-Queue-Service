using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MQS.NetCore2.Server.Code
{
    [DataContract(Namespace = "")]
    public class ClientConfigData
    {

        [DataMember]
        public int WaitObjectTime { get; set; }
        
        /// <summary>
        ///   Compact Long Heap Objects with Garbage Collection 
        /// </summary>
        [DataMember]
        public bool Memory_Compact { get; set; }

        /// <summary>
        ///   Reset time ( minutes )
        /// </summary>
        [DataMember]
        public int Memory_Compact_Time { get; set; }

    }
}
