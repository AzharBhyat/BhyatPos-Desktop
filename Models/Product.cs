namespace BhyatPos.Models
{
    public class Product
    {
        // Auto Increment Id
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public string? Category { get; set; }
        public string Barcode { get; set; }
    }
}