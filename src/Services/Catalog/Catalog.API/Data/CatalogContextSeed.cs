using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public static class CatalogContextSeed
    {
        public static void SeedData<T>(this IMongoCollection<T> collection, IEnumerable<T> data)
        {
            if (!collection.Find(p => true).Any())
            {
                collection.InsertMany(data);
            }
        }

        public static IEnumerable<Product> GeExampleProducts()
        {
            // Seed Products
            return new List<Product>()
            {
                new Product
                {
                    Id = "64a1a39f29628a040b3f4b6f",
                    Name = "Iphone X",
                    Category = "Iphone",
                    Price = 10,
                    Summary = "Iphone beauty",
                    ImgFile = "image1.jpg",
                    Description = "beauty beauty beauty"
                },
                new Product
                {
                    Id = "64a1a3b11e8e30ed35c21ed0",
                    Name = "Samsung note 10",
                    Category = "Samsung",
                    Price = 19,
                    Summary = "Summary 2",
                    ImgFile = "image2.jpg",
                    Description = "Description 2"
                },
            };
        }
    }
}