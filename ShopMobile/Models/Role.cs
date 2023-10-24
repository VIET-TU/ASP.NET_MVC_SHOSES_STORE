

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ShopMobile.Models
{
	public class Role
	{
		[Key]
		public int RoleId { get; set; }
		[Required]
		public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
