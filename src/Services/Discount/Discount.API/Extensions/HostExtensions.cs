using Discount.API.Data;
using Npgsql;

namespace Discount.API.Extensions
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
                                               "ProductId VARCHAR(24) NOT NULL," +
                                               "ProductName VARCHAR(25) NOT NULL," +
                                               "Discount INTEGER NOT NULL," +
                                               "Description TEXT)").Wait();

                        dbContext.ExecuteAsync(
                            "INSERT INTO Coupon (Code, ProductId, ProductName, Discount, Description) VALUES (@Code, @ProductId, @ProductName, @Discount, @Description)"
                            , new { Code = "TKO8", ProductId = "64a1a39f29628a040b3f4b6f", ProductName = "Iphone X", Discount = 1, Description = "Discount for Iphone X"});

                        dbContext.ExecuteAsync(
                            "INSERT INTO Coupon (Code, ProductId, ProductName, Discount, Description) VALUES (@Code, @ProductId, @ProductName, @Discount, @Description)"
                            , new { Code = "TKO8", ProductId = "64a1a3b11e8e30ed35c21ed0", ProductName = "Samsung note 10", Discount = 2, Description = "Discount for Samsung note 10"});
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