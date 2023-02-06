using FarmFreshMarket_201457F.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Lib;

namespace FarmFreshMarket_201457F.Pages
{
    
    public class UserPageModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _http;


        public UserPageModel(IHttpContextAccessor http,UserManager<User> userManager, IDataProtectionProvider provider)
        {
            _userManager = userManager;
            _protector = provider.CreateProtector("credit_card_protector");
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
            var finduser = await _userManager.FindByEmailAsync(username);
            var decryptedCreditCardNumber = _protector.Unprotect(finduser.CreditCardNumber);
            finduser.CreditCardNumber = decryptedCreditCardNumber;
            retrieveuser = finduser;

            return Page();
        }
       
    }
}
