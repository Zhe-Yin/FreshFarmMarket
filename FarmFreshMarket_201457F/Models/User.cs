using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FarmFreshMarket_201457F.Models
{
    public class User : IdentityUser
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20,ErrorMessage ="Full Name cannot be longer than 20 characters")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        [RegularExpression("^[0-9]{12}$", ErrorMessage ="Credit Card Number is 12 digit long")]
        // Remember set Min & Max length to 12
        public string CreditCardNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        // Remember set Min & Max length to 8
        [RegularExpression(@"^\d{8}$",ErrorMessage ="Phone Number must contain 8 digits only")]
        public string PhoneNumber { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        
        [MaxLength(50)]
        public string? Photo { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "About Me cannot be longer than 150 characters")]
        public string AboutMe { get; set; }

    }


}
