using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using product_api.Models;

namespace product_api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductRepository(IOptions<DatabaseSettings> options)
        {
            var settings = options.Value;
            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _productsCollection = mongoDatabase.GetCollection<Product>(settings.ProductsCollectionName);
        }

        public async Task<List<Product>> GetAsync() =>
            await _productsCollection.Find(_ => true).ToListAsync();

        public async Task<Product> GetByIdAsync(string id) =>
            await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product product) =>
            await _productsCollection.InsertOneAsync(product);

        public async Task<bool> UpdateAsync(string id, Product product)
        {
            var result = await _productsCollection.ReplaceOneAsync(x => x.Id == id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _productsCollection.DeleteOneAsync(x => x.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
