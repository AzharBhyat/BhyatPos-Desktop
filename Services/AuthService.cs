using BhyatPos.Models;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using BCrypt.Net;
using System;

namespace BhyatPos.Services
{
    public class AuthService
    {
        private string DBPath;

        public AuthService()
        {
            DBPath = "C:\\Users\\ESA\\Documents\\dev\\DatabaseBasicsc#\\DatabaseBasicsc#\\Data\\bob.db";
        }

        private string GenerateSalt(int size = 16)
        {
            byte[] saltBytes = new byte[size];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }

        public void CreateUser(User user)
        {
            string userSalt = GenerateSalt();
            string passwordHash = HashPassword(user.Password, userSalt);

            using (var connection = new SqliteConnection($"Data Source={DBPath}"))
            {
                connection.Open();

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"
                    INSERT INTO Users (Username, Password, Role, Salt)
                    VALUES ($username, $passwordhash, $role, $salt);
                ";
                insertCmd.Parameters.AddWithValue("$username", user.Username);
                insertCmd.Parameters.AddWithValue("$passwordhash", passwordHash);
                insertCmd.Parameters.AddWithValue("$role", user.Role);
                insertCmd.Parameters.AddWithValue("$salt", userSalt);

                var output = insertCmd.ExecuteNonQuery();
                Console.WriteLine($"Inserted: [{output}] user/s");
            }
        }

        public bool AuthenticateUser(string userName, string password)
        {
            using (var connection = new SqliteConnection($"Data Source={DBPath}"))
            {
                connection.Open();

                var readCmd = connection.CreateCommand();
                readCmd.CommandText = @"
                    SELECT Id, Username, Password, Role, Salt
                    FROM Users 
                    WHERE Username = $username;
                ";
                readCmd.Parameters.AddWithValue("$username", userName);

                using var reader = readCmd.ExecuteReader();

                if (reader.Read())
                {
                    var storedHash = reader.GetString(2);
                    var storedSalt = reader.GetString(4);
                    var combined = password + storedSalt;

                    return BCrypt.Net.BCrypt.Verify(combined, storedHash);
                }

                return false;
            }
        }

        // EDIT USER
        // INVOKE BACKDOOR ADMIN - OPERATOR ACCOUNT
        // LISCENCE VERIFICATION
    }
}
