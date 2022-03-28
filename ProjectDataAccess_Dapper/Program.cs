using Dapper;
using ProjectDataAccess_Dapper.Models;
using System;
using System.Data.SqlClient;

namespace ProjectDataAccess_Dapper
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = @"Server=DESKTOP-6OMQR12\SQLEXPRESS;Database=MXTarget;Integrated Security=True";

            using (var connection = new SqlConnection(connectionString))
            {
                var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
                foreach (var category in categories)
                {
                    Console.WriteLine($"{category.Id} - {category.Title}");
                    Console.Read();
                }

            }
        }
    }
}
