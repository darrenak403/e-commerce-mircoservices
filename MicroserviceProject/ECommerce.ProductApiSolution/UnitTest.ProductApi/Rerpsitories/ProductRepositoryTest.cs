using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

namespace UnitTest.ProductApi.Rerpsitories
{
    public class ProductRepositorytest
    {
        private readonly ProductDbContext productDbContext;
        private readonly ProductRepository productRepository;

        public ProductRepositorytest()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            productDbContext = new ProductDbContext(options);
            productRepository = new ProductRepository(productDbContext);
        }

        //CREATE PRODUCT
        [Fact]
        public async Task CreateAsync_WhenProductAlreadyExist_ReturnErrorResponse()
        {
            //Arrange
            var existingProduct = new Product { Name = "ExistingProduct" };
            productDbContext.Products.Add(existingProduct);
            await productDbContext.SaveChangesAsync();


            //Act
            var result = await productRepository.CreateAsync(existingProduct);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("ExistingProduct already added");
        }

        [Fact]
        public async Task CreateAsync_WhenProductDoesNotExist_AddProductAndReturnsSuccessResponse()
        {
            //Arrange
            var newProduct = new Product() { Name = "NewProduct" };

            //Act
            var result = await productRepository.CreateAsync(newProduct);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("NewProduct added to database successfully");
        }

        //DELETE PRODUCT
        [Fact]
        public async Task DeleteAsync_WhenProductIsFound_ReturnsSuccessResponse()
        {
            //Arrange
            var productToDelete = new Product { Id = 1, Name = "Existing Product", Price = 70m, Quantity = 10 };
            productDbContext.Products.Add(productToDelete);

            //Act
            var result = await productRepository.DeleteAsync(productToDelete);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Existing Product is deleted successfully");
        }

        [Fact]
        public async Task DeleteAsync_WhenProductIsNotFound_ReturnsNotFoundResponse()
        {
            //Arrange
            var productToDelete = new Product() { Id = 2, Name = "Non-Existing Product" };

            //Act
            var result = await productRepository.DeleteAsync(productToDelete);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Non-Existing Product not found");
        }

        //GET PRODUCT BY ID
        [Fact]
        public async Task FindByIdAsync_WhenProductIsFound_ReturnsProduct()
        {
            //Arrange
            var product = new Product { Id = 100, Name = "Existing Product", Price = 50m, Quantity = 5 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            //Act
            var result = await productRepository.FindByIdAsync(product.Id);

            //Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(100);
            result.Name.Should().Be("Existing Product");
        }

        [Fact]
        public async Task FindByIdAsync_WhenProductIsNotFound_ReturnsNull()
        {
            //Arrange
            var nonExistingProductId = 999;
            //Act
            var result = await productRepository.FindByIdAsync(nonExistingProductId);
            //Assert
            result.Should().BeNull();
        }

        //GET ALL PRODUCTS
        [Fact]
        public async Task GetAllAsync_WhenProductsAreFound_ReturnsProducts()
        {
            //Arrange
            var products = new List<Product>
            {
                new() { Id = 88, Name = "Product 88", Price = 20m, Quantity = 2 },
                new() { Id = 89, Name = "Product 89", Price = 220m, Quantity = 22 },
            };
            productDbContext.Products.AddRange(products);
            await productDbContext.SaveChangesAsync();

            //Act

            var result = await productRepository.GetAllAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(p => p.Name == "Product 88");
            result.Should().Contain(p => p.Name == "Product 89");
        }

        [Fact]
        public async Task GetAllAsync_WhenProductsAreNotFound_ReturnNull()
        {
            //Arrange
            //Act
            var result = await productRepository.GetAllAsync();
            //Assert
            result.Should().BeNull();
        }

        //GET BY ANT TYPE (INT, STRING, BOOL, ETC...)
        [Fact]
        public async Task GetByAsync_WhenProductIsFound_ReturnProduct()
        {
            //Arrange
            var product = new Product() { Id = 78, Name = "Product 78", Price = 100m, Quantity = 10 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();
            Expression<Func<Product, bool>> predicate = p => p.Name == "Product 78";

            //Act
            var result = await productRepository.GetByAsync(predicate);

            //Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Product 78");
        }

        [Fact]
        public async Task GetByAsync_WhenProductIsNotFound_ReturnNull()
        {
            //Arrange
            Expression<Func<Product, bool>> predicate = p => p.Name == "Product 3";

            //Act
            var result = await productRepository.GetByAsync(predicate);

            //Assert
            result.Should().BeNull();
        }


        //UPDATE PRODUCT
        [Fact]
        public async Task UpdateProduct_WhenProductIsUpdateSuccessfully_ReturnSuccessResponse()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Product Updating", Price = 50m, Quantity = 5 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            //Act
            var result = await productRepository.UpdateAsync(product);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Product Updating updated successfully");
        }

        [Fact]
        public async Task UpdateAsync_WhenProductIsNotFound_ReturnErrorResponse()
        {
            //Arrange
            var product = new Product() { Id = 3, Name = "Product 22" };

            //Act
            var result = await productRepository.UpdateAsync(product);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Product 22 not found");
        }

    }
}