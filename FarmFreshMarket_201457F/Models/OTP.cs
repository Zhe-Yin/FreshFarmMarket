using System.ComponentModel.DataAnnotations;

namespace FarmFreshMarket_201457F.Models
{
    public class OTP
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public bool Status { get; set; }
    }
}
