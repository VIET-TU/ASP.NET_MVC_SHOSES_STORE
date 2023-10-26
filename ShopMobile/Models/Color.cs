using System.ComponentModel.DataAnnotations;

namespace ShopMobile.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required]
        public int ColorName { get; set; }
    }
}
