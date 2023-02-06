using FarmFreshMarket_201457F.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FarmFreshMarket_201457F.Pages
{
    public class GoogleLogin : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly IAuthenticationService _authenticationService;

        public GoogleLogin(SignInManager<User> signInManager, ILogger<GoogleLogin> logger,IAuthenticationService authenticationService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _authenticationService = authenticationService;
        }
        public async Task<IActionResult> OnGet(bool isPersistent)
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, isPersistent, true);

            if (result.Succeeded)
            {
               
                return Redirect("/Index");
            }
            return Redirect("/");
        }
    }
}
