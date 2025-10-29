using ECommerce.ShareLibrary.Responses;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Presentation.Controllers;
namespace UnitTest.ProductApi.Controllers
{
    public class ProductControllerTest
    {
        private readonly IProduct productInterface;
        private readonly ProductController productController;

        public ProductControllerTest()
        {
            //Set up dependencies
            productInterface = A.Fake<IProduct>();

            //Set up System Under Test - SUT
            productController = new ProductController(productInterface);
        }

        //GET ALL PRODUCTS  
        [Fact]
        public async Task GetProducts_WhenProductExists_ReturnOkResponseWithProduct()
        {
            //Arrange
            var product = new List<Product>()
            {
                new() { Id = 1, Name = "Product 1", Quantity = 10, Price = 100.70m },
                new() { Id = 2, Name = "Product 2", Quantity = 110, Price = 1004.70m },
            };

            //Setup fake response for GetAllAsync method
            A.CallTo(() => productInterface.GetAllAsync()).Returns(product);

            //Act
            var result = await productController.GetProducts();

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnProducts = okResult.Value as IEnumerable<ProductDTO>;
            returnProducts.Should().NotBeNull();
            returnProducts.Should().HaveCount(2);
            returnProducts!.First().Id.Should().Be(1);
            returnProducts!.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task GetProducts_WhenNoProductExist_ReturnNotFoundResponse()
        {
            //Arrange
            var products = new List<Product>();

            //Set up fake response for GetAllAsync()
            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);

            //Act
            var result = await productController.GetProducts();

            //Assert 
            var nonFoundResult = result.Result as NotFoundObjectResult;
            nonFoundResult.Should().NotBeNull();
            nonFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = nonFoundResult.Value as string;
            message.Should().Be("No products detected in the database");
        }

        //CREATE PRODUCT
        [Fact]
        public async Task CreateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            productController.ModelState.AddModelError("Name", "The Name field is required.");

            //Act
            var result = await productController.CreateProduct(productDTO);

            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateProduct_WhenCreateIsSuccessfull_ReturnOkResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            var response = new Response(true, "Created");

            //Set up fake response for CreateAsync method
            A.CallTo(() => productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);

            //Act   
            var result = await productController.CreateProduct(productDTO);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult!.Message.Should().Be("Created");
            responseResult!.Flag.Should().BeTrue();
        }

        [Fact]
        public async Task CreateProduct_WhenCreateFails_ReturnBadRequestResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            var response = new Response(false, "Creation Failed");

            //Set up fake response for CreateAsync method
            A.CallTo(() => productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);

            //Act
            var result = await productController.CreateProduct(productDTO);

            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as Response;
            responseResult.Should().NotBeNull();
            responseResult!.Message.Should().Be("Creation Failed");
            responseResult!.Flag.Should().BeFalse();
        }

        //UPDATE PRODUCT
        [Fact]
        public async Task UpdateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            productController.ModelState.AddModelError("Price", "The Price field is required.");
            //Act
            var result = await productController.UpdateProduct(productDTO);
            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }


        [Fact]
        public async Task UpdateProduct_WhenModelStateIsSuccessfull_ReturnOkResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            var response = new Response(true, "Updated");

            //Set up fake response for UpdateAsync method
            A.CallTo(() => productInterface.UpdateAsync(A<Product>.Ignored)).Returns(response);

            //Act
            var result = await productController.UpdateProduct(productDTO);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult.Should().NotBeNull();
            responseResult!.Message.Should().Be("Updated");
            responseResult!.Flag.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateProduct_WhenUpdateFails_ReturnBadRequestResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            var response = new Response(false, "Update Failed");

            //Set up fake response for UpdateAsync method
            A.CallTo(() => productInterface.UpdateAsync(A<Product>.Ignored)).Returns(response);

            //Act
            var result = await productController.UpdateProduct(productDTO);

            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as Response;
            responseResult.Should().NotBeNull();
            responseResult!.Message.Should().Be("Update Failed");
            responseResult!.Flag.Should().BeFalse();
        }

        //DELETE PRODUCT 
        [Fact]
        public async Task DeleteProduct_WhenDeleteIsSuccessful_ReturnOkResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            var response = new Response(true, "Deleted successfully");

            //Set up fake response for DeleteAsync method
            A.CallTo(() => productInterface.DeleteAsync(A<Product>.Ignored)).Returns(response);

            //Act
            var result = await productController.DeleteProduct(productDTO);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult.Should().NotBeNull();
            responseResult!.Message.Should().Be("Deleted successfully");
            responseResult!.Flag.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteProduc_WhenDeleteFails_ReturnBadRequest()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            var response = new Response(false, "Delete Failed");

            //Set up fake response for DeleteAsync method
            A.CallTo(() => productInterface.DeleteAsync(A<Product>.Ignored)).Returns(response);

            //Act
            var result = await productController.DeleteProduct(productDTO);

            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as Response;
            responseResult.Should().NotBeNull();
            responseResult!.Message.Should().Be("Delete Failed");
            responseResult!.Flag.Should().BeFalse();
        }
    }
}