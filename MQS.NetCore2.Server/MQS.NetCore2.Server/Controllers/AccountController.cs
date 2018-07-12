using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MQS.NetCore2.Server.Models;
using MQS.NetCore2.Server.Code;

using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        // Cookie authentication in ASP.NET Core 2 without ASP.NET Identity
        // https://www.meziantou.net/2017/06/22/cookie-authentication-in-asp-net-core-2-without-asp-net-identity

        private readonly ILogger _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [ValidateRecaptcha]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var isValid = model.User == "1000" && model.Password == "1000"; // TODO Validate the username and the password with your own logic
                if (!isValid)
                {
                    ModelState.AddModelError("", "username or password is invalid");
                    return View(model);
                }

                _logger.LogInformation("User logged in.");

                // Create the identity from the user info
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.User.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, model.User.ToString()));

                // Authenticate using the identity
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberMe });

                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
