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
                CreateManyCategory(connection);
                //DeleteCategory(connection);
                //UpdateCategory(connection);
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
            Console.ReadKey();
        }

        static void CreateManyCategory(SqlConnection connection)
        {
            var category1 = new Category();
            category1.Id = Guid.NewGuid();
            category1.Title = "Csharp Master";
            category1.Url = "sharp";
            category1.Summary = "Sharp pLUS";
            category1.Order = 8;
            category1.Description = "Categoria para Csharp Master Plus";
            category1.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Azure Premium";
            category2.Url = "azure";
            category2.Summary = "Azure Cloud Computing";
            category2.Order = 8;
            category2.Description = "Categoria para o Azure Premium Cloud";
            category2.Featured = false;

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
            var rows = connection.Execute(insertSql, new[]
            {
                new {
                category1.Id,
                category1.Title,
                category1.Url,
                category1.Summary,
                category1.Order,
                category1.Description,
                category1.Featured,
            },
                 new {
                category2.Id,
                category2.Title,
                category2.Url,
                category2.Summary,
                category2.Order,
                category2.Description,
                category2.Featured,
            }
            });
            Console.WriteLine($"{rows} linhas afetadas");
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

        static void DeleteCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE [CATEGORY] WHERE[Id]=@id";

            var rows = connection.Execute(deleteQuery, new
            {
                id = new Guid("06D73E6B-315F-4CFC-B462-F643E1A50E97"),
            });

            Console.WriteLine($"{rows} registros atualizados");
        }
    }


}
