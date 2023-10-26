using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopMobile.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ProductId { set; get; }

        [Required]
        public string title { set; get; }

        [Required]
        public string description { set; get; }

        [Required]
        public string price { set; get; }

        [Required]
        public int quantity { set; get; }

        [Required]
        public string color { set; get; }

        [Required]
        public string images { set; get; }

     
        public int? CategoryId { set; get; }


        public Category? Category { get; set; }

		public int? UserId { get; set; }

        public User? User { get; set; }



    }
}
