
using FarmFreshMarket_201457F.Models;
using FarmFreshMarket_201457F.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web.Lib;

namespace FarmFreshMarket_201457F.Pages
{

    public class LoginModel : PageModel
    {


        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        //session
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly GoogleCaptchaService _captchaservice;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly OTPService _oTPService;
        public LoginModel(SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, GoogleCaptchaService captchaservice,UserManager<User> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration,IEmailService emailService,OTPService oTPService)
        {
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _captchaservice = captchaservice;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _oTPService = oTPService;
        }

        [BindProperty]
        public Login LModel { get; set; }

        
        [AllowAnonymous]
        public void OnGet()
        {
           
        }

      
        public async Task<IActionResult> OnPostGoogle()
        {
            return Challenge(_signInManager.ConfigureExternalAuthenticationProperties("Google", "/GoogleLogin"), "Google");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostInHouse()
        {


            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model is INVALID");
                return Page();
            }

                Console.WriteLine("Signing in");
            var identityResult = await _signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,LModel.RememberMe, true);
            if (identityResult.Succeeded)
            {
                Console.WriteLine("Success");

                var finduser = await _userManager.FindByEmailAsync(LModel.Email);
                _httpContextAccessor.HttpContext.Session.SetString(SessionVariable.UserName, LModel.Email);

                await GenerateOTPTokenAsync(finduser);


                return RedirectToPage("OTP");
            }
            // Account lockout
            else if (identityResult.IsLockedOut)
            {
               
                ModelState.AddModelError("", "The account is locked out");
                return Page();
            }
            Console.WriteLine("Failed");
            ModelState.AddModelError(nameof(LModel.Email), "Username or Password is incorrect");
            return Page();
        }

        public async Task GenerateOTPTokenAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendOTP(user, token);
            }
        }

        private async Task SendOTP(User user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:OTP").Value;

            Random rnd = new Random();
            int sixDigitNumber = rnd.Next(100000, 999999);
            string OTP = sixDigitNumber.ToString();

            OTP otp = new OTP()
            {
                UserId = user.Id,
                Password = OTP,
                Status = true
            };

            _oTPService.AddOTP(otp);

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                    new KeyValuePair<string, string>("{{OTP}}",
                        string.Format(OTP))
                }
            };

            await _emailService.SendEmailForOTP(options);

        }





    }
}
