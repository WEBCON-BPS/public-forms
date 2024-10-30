using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WEBCON.FormsGenerator.Presentation.Configuration.Model;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IReadOnlyConfiguration configuration;
        private readonly ILogger<AccountController> logger;
        private readonly IStringLocalizer<AccountController> localizer;

        public AccountController(IReadOnlyConfiguration configuration, ILogger<AccountController> logger, IStringLocalizer<AccountController> localizer)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.localizer = localizer;
        }
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var password = configuration.AdditionalSettings.Password;
                var login = configuration.AdditionalSettings.Login;
                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(login))
                {
                    ModelState.AddModelError("", localizer["Login config section is not configured"]);
                    return View();
                }
                if (loginViewModel.Login == login && loginViewModel.Password == password)
                {
                    var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, login));
                    identity.AddClaim(new Claim(ClaimTypes.Name, login));

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    logger?.LogInformation($"User {login} is logged");
                    return RedirectToAction("Index", "Forms");
                }
                else
                {
                    ModelState.AddModelError("", localizer["Invalid credentials"]);
                    logger?.LogWarning("Tried to login with invalid credentials");
                    return View();
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            logger?.LogInformation($"User logout");
            return RedirectToAction("Index", "Forms");
        }
    }
}
