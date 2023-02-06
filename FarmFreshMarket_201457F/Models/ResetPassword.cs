using System.ComponentModel.DataAnnotations;

namespace FarmFreshMarket_201457F.Models
{
	public class ResetPassword
	{
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,}$", ErrorMessage = "Use a minimum of 12 characters, including lower-case and upper-case , numbers and special characters")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password1 { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password2 { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime ResetTime { get; set; }

        public bool IsSuccess { get; set; }
    }
}
