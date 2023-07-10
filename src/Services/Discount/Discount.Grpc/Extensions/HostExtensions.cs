using Discount.Grpc.Data;
using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static WebApplication MigrateData(this WebApplication app, int retryCount = 0)
        {
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<IDiscountContext>();
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Migrating DB starting");
                try
                {
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
                    logger.LogInformation("Migrated DB");
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    if (retryCount < 20)
                    {
                        Thread.Sleep(2000);
                        logger.LogInformation($"Retry: {++retryCount}");
                        MigrateData(app, retryCount);
                    }
                }
            }

            return app;
        }
    }
}