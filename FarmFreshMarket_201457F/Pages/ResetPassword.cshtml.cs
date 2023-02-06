using FarmFreshMarket_201457F.Models;
using FarmFreshMarket_201457F.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FarmFreshMarket_201457F.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _http;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ResetPasswordService _resetPasswordService;
        public ResetPasswordModel(IHttpContextAccessor http, UserManager<User> userManager, IEmailService emailService, IConfiguration configuration,ResetPasswordService resetPasswordService)
        {
            _userManager = userManager;
            _http = http;
            _emailService = emailService;
            _configuration = configuration;
            _resetPasswordService = resetPasswordService;

        }

        [BindProperty,Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,}$", ErrorMessage = "Use a minimum of 12 characters, including lower-case and upper-case , numbers and special characters")]
        public string NewPassword { get; set; }

        [BindProperty,Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        [BindProperty, Required]
        public string UserId { get; set; }

        [BindProperty, Required]
        public string Token { get; set; }
        public bool IsSuccess { get; set; }

      
        public void OnGet(string uid,string token)
        {
            UserId = uid;
            Token = token;
            


        }

        public async Task<IActionResult> OnPostAsync()
        {
            //ResetModel.Token = Token;
            //ResetModel.UserId = UserId;
            if (ModelState.IsValid)
            {
                var user = await _resetPasswordService.GetPasswordReset(UserId);
                //var finduser = await _userManager.FindByIdAsync(UserId);
                
                if (user != null)
                {
                    var checkhash1 = _userManager.PasswordHasher.VerifyHashedPassword(null, user.NewPassword, NewPassword);
                    var checkhash2 = _userManager.PasswordHasher.VerifyHashedPassword(null, user.Password1, NewPassword);
                    var checkhash3 = _userManager.PasswordHasher.VerifyHashedPassword(null, user.Password2, NewPassword);

                    var newhash = _userManager.PasswordHasher.HashPassword(null, NewPassword);
                    Console.WriteLine(newhash);
                    Console.WriteLine(user.NewPassword);
                    if(checkhash1 != PasswordVerificationResult.Success && checkhash2 != PasswordVerificationResult.Success && checkhash3 != PasswordVerificationResult.Success)
                    { 
                   
                        var result = await ResetPasswordAsync(UserId, Token, NewPassword);
                        if (result.Succeeded)
                        {
                            user.Password2 = user.Password1;
                            user.Password1 = user.NewPassword;
                            user.NewPassword = newhash;
                            user.ResetTime = DateTime.Now;
                                
                            _resetPasswordService.UpdateResetPassword(user);
                            

                            ModelState.Clear();
                            IsSuccess = true;



                            return Page();
                        }
                        else
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
                       

                    }
                    else
                    {
                        ModelState.AddModelError("","New Password cannot be same as the Old Password");
                        return Page();
                    }
                }

                //ResetModel.Token = ResetModel.Token.Replace(' ','+');

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

        //public async Task GenerateForgotPasswordTokenAsync(User user)
        //{
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        await SendForgotPasswordEmail(user, token);
        //    }
        //}

        //private async Task SendForgotPasswordEmail(User user, string token)
        //{
        //    string appDomain = _configuration.GetSection("Application:AppDomain").Value;
        //    string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

        //    UserEmailOptions options = new UserEmailOptions
        //    {
        //        ToEmails = new List<string>() { user.Email },
        //        PlaceHolders = new List<KeyValuePair<string, string>>()
        //        {
        //            new KeyValuePair<string, string>("{{UserName}}", user.UserName),
        //            new KeyValuePair<string, string>("{{Link}}",
        //                string.Format(appDomain + confirmationLink, user.Id, token))
        //        }
        //    };

        //    await _emailService.SendEmailForForgotPassword(options);

        //}
        public async Task<IdentityResult> ResetPasswordAsync(string id, string token , string newpassword)
        {
            
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(id), token, newpassword);
        }

    }
}
