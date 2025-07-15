using System;

namespace BhyatPos.Models
{
    public class Sale
    {
        public int SaleID { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string? UserName { get; set; }
        public int UserID { get; set; }
        public int? CustomerID { get; set; }
        public decimal TotalPrice { get; set; }
    }
}