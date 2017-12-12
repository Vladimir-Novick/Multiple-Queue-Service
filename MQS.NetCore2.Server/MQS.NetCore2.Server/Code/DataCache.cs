
using MQS.NetCore2.Api;
using MQS.NetCore2.Code;
using System;
using System.Collections.Concurrent;
using TaskContainerLib;

namespace MQS.NetCore2.Server.Code
{
    public class DataCache
    {
        public  static TaskContainerManager TaskContainer = new TaskContainerManager();
        public static ConcurrentDictionary<String, QueueMainObject> UserDataCache = new ConcurrentDictionary<string, QueueMainObject>();



        public static QueueMainObject CreateKey(string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length == 0)
            {
                return null;
            }
            QueueMainObject ItemResultMain = new QueueMainObject();
            ItemResultMain.Key = key;

            return ItemResultMain;
        }

        public static bool GetStorageObject(QueryObject QueryObject, ref StorageObject storageResult)
        {
            bool ok;
            QueueMainObject itemResultMain = null;
            ok = DataCache.UserDataCache.TryGetValue(QueryObject.Key, out itemResultMain);
            if (ok)
            {
                bool ok2 = itemResultMain.Queue.TryDequeue(out storageResult);
                if (ok2)
                {
                    QueryObject.Status = QueueStatus.Ongoing;
                    QueryObject.DataObject = storageResult;
                }
                else
                {
                    QueryObject.Status = QueueStatus.CompleteMayBeDisposed;
                }
            }
            else
            {
                QueryObject.Status = QueueStatus.CompleteMayBeDisposed;
            }

            return ok;
        }

        public static bool SetDataObject(StorageObject dataObject)
        {
            bool ok;
            QueueMainObject ItemResultMain;
            ok = DataCache.UserDataCache.TryGetValue(dataObject.Key, out ItemResultMain);

            if (!ok)
            {
                ItemResultMain = CreateKey(dataObject.Key);
                ok = DataCache.UserDataCache.TryAdd(dataObject.Key, ItemResultMain);
            }

            ItemResultMain.Queue.Enqueue(dataObject);
            return ok;
        }


    }
}
