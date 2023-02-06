using System.ComponentModel.DataAnnotations;

namespace FarmFreshMarket_201457F.Models
{
    public class ForgotPassword
    {
        [Required, EmailAddress, Display(Name ="Registered email address")]
        public string Email { get; set; }

        public bool EmailSent { get; set; } = false;


    }
}
