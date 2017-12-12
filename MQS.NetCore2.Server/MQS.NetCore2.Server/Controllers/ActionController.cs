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
                    ok = DataCache.GetStorageObject(QueryObject, ref storageResult);
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
                    ok = DataCache.SetDataObject(dataObject);

                });

                task.Start();

                await task;
            }
            catch (Exception) {
                return false;

            }

            return ok;
        }


    }
}
