
using MQS.NetCore2.Code;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskContainerLib;

namespace MQS.NetCore2.Server.Code
{
    public class DataCache
    {
        public  static TaskContainerManager TaskContainer = new TaskContainerManager();
        public static ConcurrentDictionary<String, QueueMainObject> UserDataCache = new ConcurrentDictionary<string, QueueMainObject>();
    }
}
