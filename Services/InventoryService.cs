using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BhyatPos.Models;
using Microsoft.Data.Sqlite;

namespace BhyatPos.Services
{
    public class InventoryService : DBService
    {
        public InventoryService() { 
        }

        public void UpdateInventory(List<SaleItem> items)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "UPDATE Inventory SET Quantity = Quantity - $quantity WHERE ProductID = $productid";
                command.Parameters.Add("$quantity", SqliteType.Integer);
                command.Parameters.Add("$productid", SqliteType.Integer);

                foreach (var item in items)
                {
                    command.Parameters["$quantity"].Value = item.QuantitySold;
                    command.Parameters["$productid"].Value = item.ProductID;
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}
