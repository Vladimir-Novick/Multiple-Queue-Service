using Microsoft.AspNetCore.Mvc;
using MQS.NetCore2.Server.Code;
using System.Threading.Tasks;
using MQS.NetCore2.Code;
using System;
using MQS.NetCore2.Api;

namespace MQS.NetCore2.Server.Controllers
{

    public class ActionController : Controller
    {

        [HttpPost]
        public async Task<IActionResult> GetData([FromBody]QueryObject QueryObject)
        {
            
            bool ok = false;
            StorageObject storageResult = null;
            try
            {
                var task = new Task(() =>
                {
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
                });
                task.Start();

                await task;
            } catch (Exception ex)
            {
                QueryObject.Status = QueueStatus.Error;
                QueryObject.ErrorMessage = ex.Message;
            }

            return Ok(QueryObject);
        }

        [HttpPost]
        public async Task<bool> SetData([FromBody]StorageObject dataObject)
        {
            bool ok = false;
            QueueMainObject CreateKey(string key)
            {
                if (string.IsNullOrEmpty(key) || key.Length == 0)
                {
                    return null;
                }
                QueueMainObject ItemResultMain = new QueueMainObject();
                ItemResultMain.Key = key;

                return ItemResultMain;
            }

            try
            {

                if (dataObject == null
                    || dataObject.Key == null
                    || dataObject.Key.Length == 0
                    || dataObject.Data == null
                    || dataObject.Data.Length == 0)
                {
                    return ok;
                }
                var task = new Task(() =>
                {
                    QueueMainObject ItemResultMain;
                    ok = DataCache.UserDataCache.TryGetValue(dataObject.Key, out ItemResultMain);

                    if (!ok)
                    {
                        ItemResultMain = CreateKey(dataObject.Key);
                      ok =    DataCache.UserDataCache.TryAdd(dataObject.Key, ItemResultMain);
                    } 

                    ItemResultMain.Queue.Enqueue(dataObject);

                });

                task.Start();

                await task;
            }
            catch (Exception) { }

            return ok;
        }

    }
}
