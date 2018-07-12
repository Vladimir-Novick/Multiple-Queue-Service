using Microsoft.AspNetCore.Mvc;
using MQS.NetCore2.Server.Code;
using System.Threading.Tasks;
using MQS.NetCore2.Code;
using System;
using MQS.NetCore2.Api;
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
