using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MQS.NetCore2.Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace MQS.NetCore2.Server.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
