using System;
using product_api.Models;

namespace product_api.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(string id);
        Task CreateAsync(Product product);
        Task<bool> UpdateAsync(string id, Product product);
        Task<bool> DeleteAsync(string id);
    }
}



