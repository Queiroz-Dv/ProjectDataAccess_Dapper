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
                UpdateCategory(connection);
                ListCategories(connection);
                //CreateCategory(connection);
            }

        }


        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
                Console.ReadKey();
            }
        }

        static void CreateCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Order = 8;
            category.Description = "Categoria para o AWS";
            category.Featured = false;

            var insertSql = @"INSERT INTO 
                                    [CATEGORY] 
                             VALUES(
                                   @Id, 
                                    @Title, 
                                    @Url, 
                                    @Summary,
                                    @Order,
                                    @Description,
                                    @Featured)";

            //Retorna só um int que é a quantidade de linhas afetadas
            var rows = connection.Execute(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured,
            });
            Console.WriteLine($"{rows} linhas afetadas");
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE[Id]=@id";

            var rows = connection.Execute(updateQuery, new
            {
                id = new Guid("06D73E6B-315F-4CFC-B462-F643E1A50E97"),
                title = "Frontend 2022"
            });

            Console.WriteLine($"{rows} registros atualizados");
        }
    }


}
