namespace ShopMobile.Models
{
    public class Invoice_Products
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public string size { get; set; } 
        public int quantity { get; set; }
        public string totalPrice { get; set; }

        public Invoice? Invoice { get; set; }
        public Product? Product { get; set; }



    }
}
