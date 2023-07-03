using Catalog.API.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(configuration.GetValue<string>("Database:ConnectionString"));
            //settings.ClusterConfigurator = cb => 
            //{
            //    cb.Subscribe<CommandStartedEvent>(e => Console.WriteLine($"MongoDB command started: {e.Command}"));
            //    cb.Subscribe<CommandSucceededEvent>(e => Console.WriteLine($"MongoDB command succeeded: {e.CommandName}"));
            //    cb.Subscribe<CommandFailedEvent>(e => Console.WriteLine($"MongoDB command failed: {e.CommandName} - {e.Failure}"));
            //};

            var client = new MongoClient(settings);
            var collection = client.GetDatabase(configuration.GetValue<string>("Database:DatabaseName"));
            Products = collection.GetCollection<Product>(configuration.GetValue<string>("Database:CollectionName"));
            Products.SeedData<Product>(CatalogContextSeed.GeExampleProducts());
        }

        public IMongoCollection<Product> Products { get; }
    }
}