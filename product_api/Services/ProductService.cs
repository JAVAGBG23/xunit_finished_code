using System;
using product_api.Models;
using product_api.Repositories;

namespace product_api.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) =>
            _productRepository = productRepository;

        public async Task<List<Product>> GetAsync() =>
            await _productRepository.GetAsync();

        public async Task<Product> GetAsync(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            return product;
        }

        public async Task<Product> CreateAsync(Product newProduct)
        {
            await _productRepository.CreateAsync(newProduct);
            return newProduct;
        }

        public async Task UpdateAsync(string id, Product updatedProduct)
        {
            var success = await _productRepository.UpdateAsync(id, updatedProduct);
            if (!success)
                throw new KeyNotFoundException("Product not found.");
        }

        public async Task RemoveAsync(string id)
        {
            var success = await _productRepository.DeleteAsync(id);
            if (!success)
                throw new KeyNotFoundException("Product not found.");
        }
    }
}