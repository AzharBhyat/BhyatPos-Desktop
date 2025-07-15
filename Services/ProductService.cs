using BhyatPos.Models;
using Microsoft.Data.Sqlite;
using System;

namespace BhyatPos.Services
{
    public class ProductService : DBService
    {
        public void CreateProduct(Product product)
        {
            using var connection = GetConnection();
            connection.Open();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO Products (ProductName, Description, Price, CostPrice, Category, Barcode)
                VALUES ($name, $description, $price, $costprice, $category, barcode);
            ";

            insertCmd.Parameters.AddWithValue("$name", product.ProductName);
            insertCmd.Parameters.AddWithValue("$description", product.Description);
            insertCmd.Parameters.AddWithValue("$price", product.Price);
            insertCmd.Parameters.AddWithValue("$costprice", product.CostPrice);
            insertCmd.Parameters.AddWithValue("$category", product.Category);
            insertCmd.Parameters.AddWithValue("$barcode", product.Barcode);

            var output = insertCmd.ExecuteNonQuery();
            Console.WriteLine($"Inserted: [{output}] product/s");
        }


        public Product? ReadProductFromBarcode(string barcode)
        {
            using var connection = GetConnection();
            connection.Open();

            var readCmd = connection.CreateCommand();
            readCmd.CommandText = @"
                SELECT * FROM Products WHERE Barcode = $barcode;
            ";
            readCmd.Parameters.AddWithValue("$barcode", barcode);

            using var reader = readCmd.ExecuteReader();

            if (reader.Read())
            {

                return new Product
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Description = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    Category = reader.GetString(4),
                    Barcode = reader.GetString(5),
                    CostPrice = reader.GetDecimal(6)
                };
            }

            // return errors
            // implement logging system and error handling
            return null;
        }

        public void UpdateProduct(Product product)
        {
            using var connection = GetConnection();
            connection.Open();

            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = @"
                UPDATE Products
                SET ProductName = $name, Description = $description, Price = $price, CostPrice = $costprice, Category = $category, Barcode = $barcode 
                WHERE Id = $id;
            ";
            updateCmd.Parameters.AddWithValue("$name", product.ProductName);
            updateCmd.Parameters.AddWithValue("$description", product.Description);
            updateCmd.Parameters.AddWithValue("$price", product.Price);
            updateCmd.Parameters.AddWithValue("$costprice", product.CostPrice);
            updateCmd.Parameters.AddWithValue("$category", product.Category);
            updateCmd.Parameters.AddWithValue("$barcode", product.Barcode);
            updateCmd.Parameters.AddWithValue("$id", product.ProductID);

            var output = updateCmd.ExecuteNonQuery();
            Console.WriteLine($"Updated: [{output}] product/s");
        }

        public void DeleteProduct(int productId)
        {
            using var connection = GetConnection();
            connection.Open();

            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = @"DELETE FROM Products WHERE Id = $id;";
            deleteCmd.Parameters.AddWithValue("$id", productId);

            var output = deleteCmd.ExecuteNonQuery();
            Console.WriteLine($"Deleted: [{output}] product/s");
        }
    }
}
