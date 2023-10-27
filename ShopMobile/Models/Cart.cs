using System.ComponentModel.DataAnnotations;

namespace ShopMobile.Models
{



    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        
        public int UserId { get; set; }
        public User? User { get; set; }

        public bool Status { get; set; }

        public ICollection<Invoice_Products>? Invoice_Products { get; set; }
    }
}
