using System.ComponentModel.DataAnnotations;

namespace FarmFreshMarket_201457F.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public string Action { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
