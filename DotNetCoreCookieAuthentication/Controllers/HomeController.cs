using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCoreCookieAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        [Authorize(Policy = "PolicyA")]
        public IActionResult PolicyA()
        {
            return View();
        }
        public IActionResult Authenticate()
        {
            var abcClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "My Name"),
                new Claim(ClaimTypes.Email, "me@mail.co"),
                new Claim("Something", "Anything"),
            };
            var abcIdentity = new ClaimsIdentity(abcClaims, "Abc Identity");

            var driversLicenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "My Full Name"),
                new Claim(ClaimTypes.Email, "me@mail.co"),
                new Claim("Divers License No", "ABC123"),
            };
            var driversLicenseIdentity = new ClaimsIdentity(driversLicenseClaims, "Divers License Identity");

            var claimsPrincipal = new ClaimsPrincipal(new[] { abcIdentity, driversLicenseIdentity });

            HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToAction("Index");
        }
    }
}
