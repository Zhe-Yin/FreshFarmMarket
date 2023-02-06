using FarmFreshMarket_201457F.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Xamarin.Essentials;
using FarmFreshMarket_201457F.Models;

namespace FarmFreshMarket_201457F.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;

        public HomeController(IHttpContextAccessor contextAccessor, SignInManager<User> signInManager)
        {
            _httpContextAccessor = contextAccessor;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            return View();
        }



       

        //public IActionResult OnSessionExpired()
        //{

        //    _signInManager.SignOutAsync();
        //    _httpContextAccessor.HttpContext.Session.Clear();

        //    return RedirectToAction("Index", "Home");
        //}


    }
}
