using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MQS.NetCore2.Server.Models;
using System.Collections.Generic;
using MQS.NetCore2.Server.Code;
using System.Linq;

using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Authorization;
using MQS.NetCore2.Code;

namespace MQS.NetCore2.Server.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]

    public class StatusController : Controller
    {
        public IActionResult QueKeys()
        {
            return View();
        }

    }
}
