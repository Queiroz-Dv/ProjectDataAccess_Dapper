using Dapper;
using ProjectDataAccess_Dapper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ProjectDataAccess_Dapper
{
  class Program
  {
    static void Main(string[] args)
    {
      const string connectionString = @"Server=DESKTOP-6OMQR12\SQLEXPRESS;Database=MXTarget;Integrated Security=True";

      using (var connection = new SqlConnection(connectionString))
      {
        // Like(connection, "api");
        // SelectIn(connection);
        // QueryMultiple(connection);
        // OneToMany(connection);
        // OneToOne(connection);
        // ReadView(connection);
        // ExecuteScalar(connection);
        // ExecuteReadProcedure(connection);
        // ExecuteProcedure(connection);
        // CreateManyCategory(connection);
        // DeleteCategory(connection);
        // UpdateCategory(connection);
        ListCategories(connection);
        // CreateCategory(connection);
      }

    }


    static void ListCategories(SqlConnection connection)
    {
      // Usa o generics do C# 
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
      var category1 = new Category
      {
        Id = Guid.NewGuid(),
        Title = "Csharp Master",
        Url = "sharp",
        Summary = "Sharp pLUS",
        Order = 8,
        Description = "Categoria para Csharp Master Plus",
        Featured = false
      };

      var category2 = new Category
      {
        Id = Guid.NewGuid(),
        Title = "Azure Premium",
        Url = "azure",
        Summary = "Azure Cloud Computing",
        Order = 8,
        Description = "Categoria para o Azure Premium Cloud",
        Featured = false
      };

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
      var category = new Category
      {
        Id = Guid.NewGuid(),
        Title = "Amazon AWS",
        Url = "amazon",
        Summary = "AWS Cloud",
        Order = 8,
        Description = "Categoria para o AWS",
        Featured = false
      };

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

    static void ExecuteProcedure(SqlConnection connection)
    {
      var proc = "[spDeleteStudent]";
      var pars = new { StudentId = "79B82071-80A8-4E78-A79C-92C8CD1FD052" };
      var rows = connection.Execute(proc,
            pars,
            commandType: CommandType.StoredProcedure);

      Console.WriteLine($"{rows} linhas afetadas");
    }

    static void ExecuteReadProcedure(SqlConnection connection)
    {
      var proc = "[spGetCoursesByCategory]";
      var pars = new { @CategoryId = "5C349848-E717-9A7D-1241-0E6500000000" };
      var courses = connection.Query(proc,
            pars,
            commandType: CommandType.StoredProcedure);

      foreach (var item in courses)
      {
        Console.WriteLine(item.Id);
      }
    }

    static void ExecuteScalar(SqlConnection connection)
    {
      var category = new Category
      {
        Title = "Mancro Studio",
        Url = "mancro",
        Summary = "Mancro XP",
        Order = 8,
        Description = "Categoria para o Mancro Studio",
        Featured = false
      };

      var insertSql = @"INSERT INTO 
                                    [CATEGORY] 
                             OUTPUT inserted.[Id] 
                             VALUES(
                                   NEWID(), 
                                    @Title, 
                                    @Url, 
                                    @Summary,
                                    @Order,
                                    @Description,
                                    @Featured)";

      var id = connection.ExecuteScalar<Guid>(insertSql, new
      {

        category.Title,
        category.Url,
        category.Summary,
        category.Order,
        category.Description,
        category.Featured,
      });
      Console.WriteLine($"{id} linhas afetadas");
    }

    static void ReadView(SqlConnection connection)
    {
      var sql = "SELECT * FROM [vwCourses]";
      var courses = connection.Query(sql);
      foreach (var item in courses)
      {
        Console.WriteLine($"{item.Id} - {item.Title}");
      }
      Console.ReadKey();
    }

    static void OneToOne(SqlConnection connection)
    {
      var sql = @"SELECT 
                            * 
                        FROM 
                           [CareerItem]
                        INNER JOIN
                           [Course] ON [CareerItem].[CouseId] = [Course].[Id]";

      var items = connection.Query<CareerItem, Course, CareerItem>(sql,
          (careerItem, course) =>
          {
            careerItem.Course = course;
            return careerItem;
          }, splitOn: "Id");

      foreach (var item in items)
      {
        Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
      }
    }

    static void OneToMany(SqlConnection connection)
    {
      var sql = @"SELECT
                    [Career].[Id],
                    [Career].[Title],
                    [CareerItem].[CareerId] AS [Id],
                    [CareerItem].[Title]
                    FROM
	                    [Career]
                    INNER JOIN
	                    [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                    ORDER BY 
	                    [Career].[Title]";

      var careers = new List<Career>();
      var items = connection.Query<Career, CareerItem, Career>(sql,
          (career, item) =>
          {
            var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
            if (car == null) // Se não existe na lista 
            {
              car = career;
              car.Items.Add(item);
              careers.Add(car);
            }
            else
            {
              career.Items.Add(item);
            }
            return career;
          }, splitOn: "[CareerId]");

      foreach (var career in careers)
      {
        Console.WriteLine($"{career.Title}");

        foreach (var item in career.Items)
        {
          Console.WriteLine($" - {item.Title}");
        }
      }
    }

    static void QueryMultiple(SqlConnection connection)
    {
      var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

      using (var mult = connection.QueryMultiple(query))
      {
        var categories = mult.Read<Category>();
        var courses = mult.Read<Course>();

        foreach (var item in categories)
        {
          Console.WriteLine(item.Title);
        }
        foreach (var item in courses)
        {
          Console.WriteLine(item.Title);
        }
      }
    }

    static void SelectIn(SqlConnection connection)
    {
      var query = @"SELECT TOP 10 * FROM Career WHERE [Id] IN @Id";

      var items = connection.Query<Career>(query, new
      {
        Id = new[]

           {
                    "01AE8A85-B4E8-4194-A0F1-1C6190AF54CB",
                    "4327AC7E-963B-4893-9F31-9A3B28A4E72B"

                }
      });

      foreach (var item in items)
      {
        Console.WriteLine(item.Title);
        Console.ReadKey();
      }
    }

    static void Like(SqlConnection connection, string term)
    {
      var query = @"SELECT * FROM [Course] WHERE [Title] LIKE @exp";

      var items = connection.Query<Course>(query, new
      {
        exp = $"%{term}%"
      });

      foreach (var item in items)
      {
        Console.WriteLine(item.Title);
        Console.ReadKey();
      }
    }

    static void Transaction(SqlConnection connection)
    {
      var category = new Category
      {
        Id = Guid.NewGuid(),
        Title = "Ios as",
        Url = "IosAs",
        Summary = "Ios Cloud",
        Order = 8,
        Description = "Categoria para o iOS",
        Featured = false
      };

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

      using (var transaction = connection.BeginTransaction())
      {

        var rows = connection.Execute(insertSql, new
        {
          category.Id,
          category.Title,
          category.Url,
          category.Summary,
          category.Order,
          category.Description,
          category.Featured,
        }, transaction);
        //transaction.Commit();
        transaction.Rollback();
        Console.WriteLine($"{rows} linhas afetadas");
      }
    }
  }


}
