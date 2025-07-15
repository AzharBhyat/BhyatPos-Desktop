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
    public class SalesService : DBService
    {
        public SalesService() { 
        }
        public int CreateSale(Sale sale)
        {
            using var connection = GetConnection();
            connection.Open();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
            INSERT INTO Sales (InvoiceNumber, UserName, UserID, CustomerID, TotalPrice)
            VALUES ($invoicenumber, $username, $userID, $customerID, $totalprice);
            SELECT last_insert_rowid();
        ";
            insertCmd.Parameters.AddWithValue("$invoicenumber", sale.InvoiceNumber);
            insertCmd.Parameters.AddWithValue("$username", sale.UserName);
            insertCmd.Parameters.AddWithValue("$userID", sale.UserID);
            insertCmd.Parameters.AddWithValue("$customerID", sale.CustomerID);
            insertCmd.Parameters.AddWithValue("$totalprice", sale.TotalPrice);

            // ExecuteScalar returns the first column of the first row (the SaleID)
            var saleId = Convert.ToInt32(insertCmd.ExecuteScalar());
            Console.WriteLine($"Inserted sale with SaleID: {saleId}");
            return saleId;
        }

        public void CreateSaleItems(List<SaleItem> items, int saleID)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO SaleItems (SaleID, ProductID, QuantitySold, PriceAtSale, Subtotal)
                VALUES ($saleid, $productid, $quantitysold, $priceatsale, $subtotal)";

                command.Parameters.Add("$saleid", SqliteType.Integer);
                command.Parameters.Add("$productid", SqliteType.Integer);
                command.Parameters.Add("$quantitysold", SqliteType.Integer);
                command.Parameters.Add("$priceatsale", SqliteType.Real);
                command.Parameters.Add("$subtotal", SqliteType.Real);

                foreach (var item in items)
                {
                    command.Parameters["$saleid"].Value = saleID;
                    command.Parameters["$productid"].Value = item.ProductID;
                    command.Parameters["$quantitysold"].Value = item.QuantitySold;
                    command.Parameters["$priceatsale"].Value = item.PriceAtSale;
                    command.Parameters["$subtotal"].Value = item.Subtotal;
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

        public bool ProcessSale(Sale sale, List<SaleItem> items)
        {
            try
            {
                int saleID = CreateSale(sale);
                CreateSaleItems(items, saleID);
                
                InventoryService inventoryService = new InventoryService();
                inventoryService.UpdateInventory(items);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }


    }
}
