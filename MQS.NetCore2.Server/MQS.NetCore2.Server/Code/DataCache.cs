
using MQS.NetCore2.Api;
using MQS.NetCore2.Code;
using System;
using System.Collections.Concurrent;
using TaskContainerLib;
/*

Copyright (C) 2016-2018 by Vladimir Novick http://www.linkedin.com/in/vladimirnovick ,

    vlad.novick@gmail.com , http://www.sgcombo.com , https://github.com/Vladimir-Novick
	

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/
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
