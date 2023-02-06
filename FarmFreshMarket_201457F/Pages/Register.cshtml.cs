using FarmFreshMarket_201457F.Models;
using FarmFreshMarket_201457F.Models;
using FarmFreshMarket_201457F.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Web.Lib;

namespace FarmFreshMarket_201457F.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<User> userManager { get; }
        private SignInManager<User> signInManager { get; }

        private IWebHostEnvironment _environment;

        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _contxt;
        private readonly ResetPasswordService _resetPasswordService;
        private readonly RoleManager<IdentityRole> _roleManager;


        [BindProperty]
        public User RModel { get; set; }

        


        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,}$",ErrorMessage = "Use a minimum of 12 characters, including lower-case and upper-case , numbers and special characters")]
        public string Password { get; set; }


        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        [BindProperty]
        public string Register { get; set; }
    

        public RegisterModel(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment,ResetPasswordService resetPasswordService, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
            _contxt = httpContextAccessor;
            _protector = provider.CreateProtector("credit_card_protector");
            _resetPasswordService = resetPasswordService;
            _roleManager = roleManager;

        }

        public void OnGet()
        {



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
          
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    var ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);


                    var protectedCreditCardNumber = _protector.Protect(RModel.CreditCardNumber);
                    Console.WriteLine(protectedCreditCardNumber);


                    var user = new User
                    {
                        UserName = EmailAddress,
                        Photo = ImageURL,
                        FullName = RModel.FullName,
                        Gender = RModel.Gender,
                        Email = EmailAddress,
                        PhoneNumber = "+65 " + RModel.PhoneNumber,
                        DeliveryAddress = RModel.DeliveryAddress,
                        CreditCardNumber = protectedCreditCardNumber,
                        AboutMe = RModel.AboutMe
                        
                    };

                  
                    var finduser = await userManager.FindByEmailAsync(EmailAddress);
                    
                    if (finduser != null)
                    {
                        ModelState.AddModelError(Register, "This email has already been registered");
                        return Page();
                    }

                    var result = await userManager.CreateAsync(user,Password);
                    if (result.Succeeded)
                    {
                        //IdentityRole role = await _roleManager.FindByNameAsync("Guest");
                        //if(role == null)
                        //{
                        //    await _roleManager.CreateAsync(new IdentityRole("Guest"));
                        //}
                        var checkuser = await userManager.FindByEmailAsync(user.Email);

                        checkuser.EmailConfirmed = true;
                        await userManager.UpdateAsync(user);

                        var newreset = new ResetPassword
                        {
                            UserId = checkuser.Id,
                            Token = "0",
                            NewPassword = user.PasswordHash,
                            Password1 = user.PasswordHash,
                            Password2 = user.PasswordHash,
                            ResetTime = DateTime.Now,
                            IsSuccess = false,
                        };
                        _resetPasswordService.AddResetPassword(newreset);

                        if(EmailAddress == "tayzheyin123@gmail.com")
                        {
                            IdentityRole role = await _roleManager.FindByNameAsync("Admin");
                            if (role == null)
                            {
                                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                            }
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                       


                        return RedirectToPage("Login");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return Page();
        }



    }
}
