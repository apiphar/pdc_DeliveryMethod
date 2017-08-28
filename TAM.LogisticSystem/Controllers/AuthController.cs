using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using TAM.Passport.SDK;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class AuthController : Controller
    {
        private readonly AuthenticationService AuthMan;
        private readonly PassportApi Passport;

        public AuthController(PassportApi passport, AuthenticationService authMan)
        {
            this.Passport = passport;
            this.AuthMan = authMan;
        }

        [Guest]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Guest]
        public async Task<IActionResult> Login(string TAMSignOnToken, string ReturnUrl)
        {
            if (string.IsNullOrEmpty(TAMSignOnToken))
            {
                ModelState.AddModelError("token", "Token is null or empty.");
                return View();
            }

            var claims = await Passport.VerifyTokenAsync(TAMSignOnToken);
            if (claims == null)
            {
                // Unable to verify token.
                ModelState.AddModelError("token", "Unable to verify authentication token to TAM Passport.");
                return View();
            }

            var valid = Passport.VerifyClaims(claims);
            if (valid == false)
            {
                // Token claims are not valid.
                ModelState.AddModelError("token", "Token issuer or audience or expiration are not valid.");
                return View();
            }

            if (claims.Roles.Any() == false)
            {
                // You have no power here.
                ModelState.AddModelError("token", "You are not authorized to access Toyota Logistic System.");
                return View();
            }

            // Create login session here.
            var user = await AuthMan.CompileToTangoUserInfo(claims);
            var login = await AuthMan.CreateLoginCookie(user);
            var options = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddDays(180),
                IsPersistent = true
            };

            await HttpContext.Authentication.SignInAsync(AuthenticationService.CookieAuthenticationScheme, login, options);

            if (string.IsNullOrEmpty(ReturnUrl) == false)
            {
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(AuthenticationService.CookieAuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
