using System;
using Moq;
using product_api.Models;
using product_api.Repositories;
using product_api.Services;

namespace product_api_tests
{
    public class ProductServiceTest
    {
        // Mock object for the IProductRepository interface
        private readonly Mock<IProductRepository> _mockRepository;

        // Instance of the ProductService being tested
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            // Initialize the mock repository
            _mockRepository = new Mock<IProductRepository>();

            // Inject the mock repository into the ProductService
            _productService = new ProductService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnAllProducts()
        {
            // Arrange: Set up the mock data and configure the repository behavior
            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product A", Color = "Red", Price = 10.0M, StockQuantity = 100 },
                new Product { Id = "2", Name = "Product B", Color = "Blue", Price = 20.0M, StockQuantity = 200 }
            };
            _mockRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(products);

            // Act: Call the method under test
            var result = await _productService.GetAsync();

            // Assert: Verify the results
            Assert.Equal(2, result.Count);                  // Check that two products are returned
            Assert.Equal("Product A", result[0].Name);      // Verify the name of the first product
            _mockRepository.Verify(repo => repo.GetAsync(), Times.Once); // Ensure GetAsync was called once
        }

        [Fact]
        public async Task GetAsync_WithValidId_ShouldReturnProduct()
        {
            // Arrange: Set up a product and configure the repository to return it
            var product = new Product { Id = "1", Name = "Product A", Color = "Red", Price = 10.0M, StockQuantity = 100 };
            _mockRepository.Setup(repo => repo.GetByIdAsync("1")).ReturnsAsync(product);

            // Act: Call the method with a valid ID
            var result = await _productService.GetAsync("1");

            // Assert: Verify the result is as expected
            Assert.NotNull(result);                         // Check that a product is returned
            Assert.Equal("Product A", result.Name);         // Verify the product's name
            _mockRepository.Verify(repo => repo.GetByIdAsync("1"), Times.Once); // Ensure GetByIdAsync was called once
        }

        [Fact]
        public async Task GetAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Configure the repository to return null for an invalid ID
            _mockRepository.Setup(repo => repo.GetByIdAsync("invalid")).ReturnsAsync((Product)null);

            // Act & Assert: Call the method and expect an exception
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.GetAsync("invalid"));
            _mockRepository.Verify(repo => repo.GetByIdAsync("invalid"), Times.Once); // Ensure GetByIdAsync was called once
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewProduct()
        {
            // Arrange: Create a new product without an ID
            var newProduct = new Product
            {
                Name = "Product C",
                Description = "Description C",
                Color = "Green",
                Price = 30.99M,
                StockQuantity = 300
            };
            // Configure the repository to complete successfully when CreateAsync is called
            _mockRepository.Setup(repo => repo.CreateAsync(newProduct)).Returns(Task.CompletedTask);

            // Act: Call the method to create the product
            var result = await _productService.CreateAsync(newProduct);

            // Assert: Verify the product was added
            Assert.Equal(newProduct, result);               // Check that the returned product matches the input
            _mockRepository.Verify(repo => repo.CreateAsync(newProduct), Times.Once); // Ensure CreateAsync was called once
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateProduct()
        {
            // Arrange: Set up the updated product and configure the repository
            var updatedProduct = new Product
            {
                Id = "1",
                Name = "Updated Product",
                Description = "Updated Description",
                Color = "Yellow",
                Price = 40.99M,
                StockQuantity = 400
            };
            _mockRepository.Setup(repo => repo.UpdateAsync("1", updatedProduct)).ReturnsAsync(true);

            // Act: Call the method to update the product
            await _productService.UpdateAsync("1", updatedProduct);

            // Assert: Verify the update was successful
            _mockRepository.Verify(repo => repo.UpdateAsync("1", updatedProduct), Times.Once); // Ensure UpdateAsync was called once
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Configure the repository to return false for an invalid ID
            var updatedProduct = new Product
            {
                Id = "invalid",
                Name = "Updated Product",
                Description = "Updated Description",
                Color = "Yellow",
                Price = 40.99M,
                StockQuantity = 400
            };
            _mockRepository.Setup(repo => repo.UpdateAsync("invalid", updatedProduct)).ReturnsAsync(false);

            // Act & Assert: Expect an exception when updating with an invalid ID
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.UpdateAsync("invalid", updatedProduct));
            _mockRepository.Verify(repo => repo.UpdateAsync("invalid", updatedProduct), Times.Once); // Ensure UpdateAsync was called once
        }

        [Fact]
        public async Task RemoveAsync_WithValidId_ShouldRemoveProduct()
        {
            // Arrange: Configure the repository to return true when deleting a valid ID
            _mockRepository.Setup(repo => repo.DeleteAsync("1")).ReturnsAsync(true);

            // Act: Call the method to remove the product
            await _productService.RemoveAsync("1");

            // Assert: Verify the product was removed
            _mockRepository.Verify(repo => repo.DeleteAsync("1"), Times.Once); // Ensure DeleteAsync was called once
        }

        [Fact]
        public async Task RemoveAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Configure the repository to return false for an invalid ID
            _mockRepository.Setup(repo => repo.DeleteAsync("invalid")).ReturnsAsync(false);

            // Act & Assert: Expect an exception when deleting with an invalid ID
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.RemoveAsync("invalid"));
            _mockRepository.Verify(repo => repo.DeleteAsync("invalid"), Times.Once); // Ensure DeleteAsync was called once
        }
    }
}


