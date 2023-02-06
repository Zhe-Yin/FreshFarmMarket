using FarmFreshMarket_201457F.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Lib;

namespace FarmFreshMarket_201457F.Pages
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _http;


        public PrivacyModel(IHttpContextAccessor http, UserManager<User> userManager)
        {
            _userManager = userManager;
            _http = http;

        }
        public User retrieveuser { get; set; } = new();
        public string username { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            username = _http.HttpContext?.Session.GetString(SessionVariable.UserName);
            if (username == null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}