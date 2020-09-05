using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AvanzaScraper
{
    public class AvanzaRepo
    {
        public static void InsertStockData(StockObject item)
        
        {
            using (IDbConnection db = new SqlConnection("Server=genericdatabase.database.windows.net;Database=genericdatabase;User Id=Felixahlcrona;Password=Lexmark123;"))
            {
                string insertQuery = @"INSERT INTO [dbo].[FuturesTable]([Name],[Type],[Price],[FetchDate]) VALUES (@Name,@Type,@Price,@FetchDate)";

                var result = db.Execute(insertQuery, new
                {
                    Name = item.Name,
                    Type = item.Type,
                    Price = item.Price,
                    FetchDate = DateTime.Now.AddHours(2)
                });
            }
        }


        public static IEnumerable<StockObject> GetData()
        {
            using (IDbConnection db = new SqlConnection("Server=genericdatabase.database.windows.net;Database=genericdatabase;User Id=Felixahlcrona;Password=Lexmark123;"))
            {
                string insertQuery = @"SELECT * FROM  [dbo].[FuturesTable]";

                var result = db.Query<StockObject>(insertQuery);
                return result;
            }
        }

        public static IEnumerable<StockObject> GetTradingDays()
        {
            using (IDbConnection db = new SqlConnection("Server=genericdatabase.database.windows.net;Database=genericdatabase;User Id=Felixahlcrona;Password=Lexmark123;"))
            {
                string insertQuery = @"SELECT FetchDate FROM[dbo].[FuturesTable]
                                    WHERE Type = 'OMXS30'
                                    ORDER BY FetchDate ASC";

                var result = db.Query<StockObject>(insertQuery);
                return result;
            }
        }



    }

}

