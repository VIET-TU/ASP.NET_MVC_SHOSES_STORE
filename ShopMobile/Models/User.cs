
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ShopMobile.Models.Role;

namespace ShopMobile.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserId { get; set; }

		[Required(ErrorMessage = "Email trống")]
		[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string email { set; get; }

		[Required(ErrorMessage = "Firtname trống")]
        [StringLength(50,MinimumLength =2)]
        public string fristName { set; get; }

		[Required(ErrorMessage = "Lastname trống")]
		[StringLength(50, MinimumLength = 2)]
		public string lastName { set; get; }

        [DefaultValue(null)]
        public string? address { set; get; }

        [Required(ErrorMessage = "Password trống")]
		public string password { set; get; }

        public string? avartar { set; get; }

        public int? RoleId { get; set; } = 2;
        

		public Role? Role { set; get; }


        public ICollection<Product>? Products { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }


    }

    public class Register : User
	{
		[Required(ErrorMessage = "Password trống")]
		public string confirmPassword { set; get; }
	}

    public class Login 
    {
		[Required(ErrorMessage = "Email trống")]
		public string email { get; set; }

		[Required(ErrorMessage = "Password trống")]
		public string password { get; set; }
	}
}

