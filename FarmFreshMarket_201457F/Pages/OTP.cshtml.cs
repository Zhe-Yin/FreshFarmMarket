using FarmFreshMarket_201457F.Models;
using FarmFreshMarket_201457F.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Web.Lib;

namespace FarmFreshMarket_201457F.Pages
{
    [Authorize]
    public class OTPModel : PageModel
    {
        
        private UserManager<User> _userManager { get; }
        private SignInManager<User> _signInManager { get; }
        private IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contxt;
        private readonly OTPService _oTPService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public OTPModel(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, OTPService oTPService,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _contxt = httpContextAccessor;
            _oTPService = oTPService;
            _roleManager = roleManager;
   
           

        }

      

        [BindProperty,Required]
        public string OTP { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = _contxt.HttpContext.Session.GetString(SessionVariable.UserName);
            var user = await _userManager.FindByEmailAsync(username);
            if (ModelState.IsValid)
            {
                IdentityRole role = await _roleManager.FindByNameAsync("User");
                if (role == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                var findOTP = await _oTPService.GetOTP(user.Id);
                if(OTP != findOTP.Password)
                {
                    ModelState.AddModelError(OTP, "Wrong Password, Please try again");
                }
                else
                {
                    //var remove = await _userManager.RemoveFromRoleAsync(user, "User");
                    //var add = await _userManager.AddToRoleAsync(user, "Guest");
                    //if (remove.Succeeded && add.Succeeded)
                    //{

                    //}
                    //else
                    //{
                    //    ModelState.AddModelError(OTP, "Pls try again");
                    //}

                    await _userManager.AddToRoleAsync(user, "User");
                    _oTPService.DeleteOTP(findOTP);
                    return RedirectToPage("Index");

                }
            }
            return Page();




        }
    }
}
