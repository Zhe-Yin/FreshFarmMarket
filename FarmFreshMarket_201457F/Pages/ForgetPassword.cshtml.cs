using FarmFreshMarket_201457F.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using FarmFreshMarket_201457F.Services;

namespace FarmFreshMarket_201457F.Pages
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _http;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ResetPasswordService _resetPasswordService;



        public ForgetPasswordModel(IHttpContextAccessor http, UserManager<User> userManager, IEmailService emailService, IConfiguration configuration, ResetPasswordService resetPasswordService)
        {
            _userManager = userManager;
            _http = http;
            _emailService = emailService;
            _configuration = configuration;
            _resetPasswordService = resetPasswordService;

        }

        [BindProperty]
        public ForgotPassword ForgetModel { get; set; }

        public bool EmailSent { get; set; }

        

        public void OnGet()
        {

        }

 
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var finduser = await _userManager.FindByEmailAsync(ForgetModel.Email);
                if(finduser != null)
				{
                    var user = await _resetPasswordService.GetPasswordReset(finduser.Id);
                    TimeSpan oneMinute = TimeSpan.FromMinutes(1);
                    if (user != null)
                    {

                        var timedifference = DateTime.Now - user.ResetTime;
                        if (timedifference > oneMinute)
                        {
                            await GenerateForgotPasswordTokenAsync(finduser);
                        }
                        else
                        {
                            ModelState.AddModelError("", "You have just recently change your password , Pls try again later.");
                            return Page();
                        }
                    }



                }
                else
                {
                    ModelState.AddModelError("", "This email has not been registered.");
                    return Page();
                }

                ModelState.Clear();
                EmailSent = true;
                ForgetModel.EmailSent = EmailSent;

            }
            else if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        Console.WriteLine($"Key: {key}");
                        foreach (var error in state.Errors)
                        {
                            Console.WriteLine($"Error: {error.ErrorMessage}");
                            ModelState.AddModelError("", error.ErrorMessage);
                        }
                    }
                }
            }

            return Page();
        }
       
        public async Task GenerateForgotPasswordTokenAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmail(user, token);
            }
        }

        private async Task SendForgotPasswordEmail(User user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgetPassword").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await _emailService.SendEmailForForgotPassword(options);

        }
    }
}
