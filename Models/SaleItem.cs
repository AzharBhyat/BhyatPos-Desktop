namespace BhyatPos.Models
{
    public class SaleItem
    {
        // Core Sale Item non-nullable
        public int SaleItemID { get; set; }
        public int ProductID { get; set; }
        public decimal PriceAtSale { get; set; }
        public int QuantitySold { get; set; }
        public decimal? Subtotal { get; set; }
        public int SaleID { get; set; }

        // Sale Item Properties for building SALE in VIEW
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal CostPrice { get; set; }
        public string? Category { get; set; }
        public string? Barcode { get; set; }

    }
}