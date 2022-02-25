using Microsoft.Extensions.Caching.Memory;

namespace Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IUnitOfWork> _uowStub = new();
        private readonly Mock<IDIFactory> factoryStub = new();

        #region GetProducts
        [Fact]
        public async Task GetProducts_WithValues_ReturnsOkObject()
        {
            // Arrange
            List<Products> products = new()
            {
                GenerateProduct(),
                GenerateProduct()
            };

            _uowStub.Setup(repo => repo.ProductRepo.GetProductsAsync())
                .ReturnsAsync(products);

            var controller = new ProductController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.GetProducts();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetProducts_WithNull_ReturnsNotFound()
        {
            // Arrange
            List<Products> products = null;

            _uowStub.Setup(repo => repo.ProductRepo.GetProductsAsync())
                .ReturnsAsync(products);

            var controller = new ProductController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.GetProducts();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion


        #region PostProduct
        [Fact]
        public async Task PostProduct_ProductExists_ReturnsCreatedAtAction()
        {
            // Arrange
            var productToCreate = GenerateProductDto();
            _uowStub.Setup(repo => repo.ProductRepo.ProductExists(It.IsAny<string>()))
                .Returns(true);
            factoryStub.Setup(x => x.Products()).Returns(GenerateProduct());

            var controller = new ProductController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.PostProduct(productToCreate);

            // Assert
            var createdProduct = (result as CreatedAtActionResult).Value as ProductsDto;

            productToCreate.Should().BeEquivalentTo(createdProduct,
                options => options.ComparingByMembers<ProductsDto>()
                .ExcludingMissingMembers());

            createdProduct.isAvailable.Should().NotBeNull();
        }

        [Fact]
        public async Task PostProduct_ProductNotExists_ReturnsBadRequest()
        {
            // Arrange
            var productToCreate = GenerateProductDto();
            _uowStub.Setup(repo => repo.ProductRepo.ProductExists(It.IsAny<string>()))
                .Returns(false);

            var controller = new ProductController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.PostProduct(productToCreate);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
        #endregion


        #region DeleteProduct
        [Fact]
        public async Task DeleteProduct_ProductNotExists_ReturnsBadRequest()
        {
            // Arrange
            _uowStub.Setup(repo => repo.ProductRepo.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((Products)null);

            var controller = new ProductController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.DeleteProduct(It.IsAny<int>());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ProductExists_ReturnsNoContent()
        {
            // Arrange
            Products product = GenerateProduct();

            _uowStub.Setup(repo => repo.ProductRepo.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(product);

            var controller = new ProductController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.DeleteProduct(product.ProductID);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion


        #region PutProduct
        [Fact]
        public async Task PutProduct_ProductNotExists_ReturnsNotFound()
        {
            // Arrange
            _uowStub.Setup(repo => repo.ProductRepo.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((Products)null);

            var controller = new ProductController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.PutProduct(It.IsAny<int>(), GenerateProductDto());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutProduct_ProductExists_ReturnsNoContent()
        {
            // Arrange
            Products product = GenerateProduct();

            _uowStub.Setup(repo => repo.ProductRepo.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(product);

            var controller = new ProductController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.PutProduct(product.ProductID, GenerateProductDto());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        public Products GenerateProduct()
        {
            return new()
            {
                Name = "Kofi",
                CategoryID = 1,
                isAvailable = true,
                Price = 23
            };
        }

        public ProductsDto GenerateProductDto()
        {
            return new()
            {
                Title = "Kofi",
                CategoryName = "Kpfi",
                isAvailable = true,
                Price = 23
            };
        }
    }
}
