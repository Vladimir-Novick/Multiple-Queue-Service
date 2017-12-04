using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MQS.NetCore2.Api
{
    [DataContract]
    [XmlType(AnonymousType = true)]
    public enum QueueStatus
    {
        New = 0,
        Ongoing = 1,
        CompleteHasDataForClient = 2,
        CompleteMayBeDisposed = 3,
        Error = 4,
        Removed = 5
    }
}
