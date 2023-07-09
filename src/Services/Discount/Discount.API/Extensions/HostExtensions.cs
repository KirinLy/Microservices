using System.Diagnostics;
using Discount.API.Data;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static WebApplication SeedData(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<IDiscountContext>();
                // check table coupon exist
                var exist = dbContext.QueryFirstOrDefaultAsync<bool>(
                    "SELECT EXISTS (SELECT 1 FROM pg_tables WHERE Tablename = @Tablename)",
                    new {Tablename = "coupon"}).Result;
                if (!exist)
                {
                    dbContext.ExecuteAsync("CREATE TABLE Coupon " +
                                           "( Id SERIAL PRIMARY KEY," +
                                           "Code VARCHAR(20) NOT NULL," +
                                           "ProductName VARCHAR(25) NOT NULL," +
                                           "Discount INTEGER NOT NULL," +
                                           "Description TEXT)").Wait();

                    dbContext.ExecuteAsync(
                        "INSERT INTO Coupon (Code, ProductName, Discount, Description) VALUES (@Code, @ProductName, @Discount, @Description)"
                        , new { Code = "TKO8", ProductName = "SamSung", Discount = 10, Description = "Discount for SamSung"});

                    dbContext.ExecuteAsync(
                        "INSERT INTO Coupon (Code, ProductName, Discount, Description) VALUES (@Code, @ProductName, @Discount, @Description)"
                        , new { Code = "TZHS98", ProductName = "IPhone", Discount = 20, Description = "Discount for Iphone"});
                }
            }

            return app;
        }
    }
}