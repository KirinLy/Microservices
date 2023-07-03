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
                    Name = "Product 1",
                    Category = "Category 1",
                    Price = 10.99m,
                    Summary = "Summary 1",
                    ImgFile = "image1.jpg",
                    Description = "Description 1"
                },
                new Product
                {
                    Id = "64a1a3b11e8e30ed35c21ed0",
                    Name = "Product 2",
                    Category = "Category 2",
                    Price = 19.99m,
                    Summary = "Summary 2",
                    ImgFile = "image2.jpg",
                    Description = "Description 2"
                },
            };
        }
    }
}