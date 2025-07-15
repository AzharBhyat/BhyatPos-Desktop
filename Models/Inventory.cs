using System;

namespace BhyatPos.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        
        //no use for now
        public string WarehouseLocation { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}