using FarmFreshMarket_201457F.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Lib;

namespace FarmFreshMarket_201457F.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _contxt;
        private readonly UserManager<User> _userManager;
        public LogoutModel(IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager,UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _contxt = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet() {
            if (User.Identity.IsAuthenticated)
            {
                var username = _contxt.HttpContext.Session.GetString(SessionVariable.UserName);
                var finduser = await _userManager.FindByEmailAsync(username);
                
               
                _signInManager.SignOutAsync();
                _contxt.HttpContext.Session.Clear();
               


                
            }
            return RedirectToPage("Index");
        }
        //public async Task<IActionResult> OnPostLogoutAsync()
        //{
        //    _signInManager.SignOutAsync();
        //    _contxt.HttpContext.Session.Clear();
        //    return RedirectToPage("Login");
        //}
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }

    }
}
