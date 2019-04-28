using Microsoft.AspNetCore.Mvc;
using byalexblog.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace byalexblog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfigurationProvider configurationProvider;
        private readonly ISettingDAO settingsDAO;

        public AccountController(IConfigurationProvider configurationProvider, ISettingDAO settingsDAO)
        {
            this.configurationProvider = configurationProvider;
            this.settingsDAO = settingsDAO;
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public async Task<RedirectToActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Article");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string key, string returnUrl)
        {
            if (key != settingsDAO.Get("Password"))
            {
                ModelState.AddModelError("key", "Key is incorrect");
                return View("Login");
            }

            var claimsIdentity = new ClaimsIdentity(
                new [] { new Claim(ClaimTypes.Role, "Administrator") }, 
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return returnUrl != null 
                ? Redirect(returnUrl) 
                : (ActionResult)RedirectToAction("Add", "Article");
        }
    }
}