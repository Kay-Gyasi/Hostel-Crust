namespace Tests
{
    public class OrderDetailControllerTests
    {
        private readonly Mock<IUnitOfWork> _uowStub = new();
        private readonly Mock<IDIFactory> factoryStub = new();

        #region GetDetails
        [Fact]
        public async Task GetOrderDetails_WithValues_ReturnsOkObjectResult()
        {
            // Arrange
            var details = new[]
            {
                GenerateDetails(),
                GenerateDetails()
            };

            _uowStub.Setup(repo => repo.DetailRepo.GetOrderDetailsAsync())
                .ReturnsAsync(details);

            var controller = new OrderDetailController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.GetOrderDetails();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderDetails_WithNullValues_ReturnsNotFound()
        {
            // Arrange
            IEnumerable<OrderDetail> details = null;

            _uowStub.Setup(repo => repo.DetailRepo.GetOrderDetailsAsync())
                .ReturnsAsync(details);

            var controller = new OrderDetailController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.GetOrderDetails();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion


        #region DeleteDetail
        [Fact]
        public async Task DeleteOrderDetails_DetailExists_ReturnsNoContent()
        {
            // Arrange
            _uowStub.Setup(repo => repo.DetailRepo.OrderDetailExists(It.IsAny<int>()))
                .Returns(true);

            var controller = new OrderDetailController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.DeleteOrderDetail(It.IsAny<int>());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOrderDetails_DetailNotExists_ReturnsBadRequest()
        {
            // Arrange
            _uowStub.Setup(repo => repo.DetailRepo.OrderDetailExists(It.IsAny<int>()))
                .Returns(false);

            var controller = new OrderDetailController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.DeleteOrderDetail(It.IsAny<int>());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        #endregion


        #region PostOrderDetail
        [Fact]
        public async Task PostOrderDetail_OrderNumExists_ReturnsCreatedObject()
        {
            // Arrange
            var detailToPost = GenerateDetails();

            _uowStub.Setup(repo => repo.DetailRepo.GetProductId(It.IsAny<string>()))
                .Returns(It.IsAny<int>());
            factoryStub.Setup(x => x.OrderDetail()).Returns(GenerateDetails());

            var controller = new OrderDetailController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.PostOrderDetail(GenerateDetailsDto());

            // Assert
            var postedDetail = (result as CreatedAtActionResult).Value as OrderDetailDto;

            detailToPost.Should().BeEquivalentTo(postedDetail,
                options => options.ComparingByMembers<OrderDetailDto>()
                .ExcludingMissingMembers());
        }
        #endregion

        #region Order details for order
        [Fact]
        public async Task GetDetailsForOrder_WithNullReturn_ReturnsBadRequest()
        {
            // Arrange
            List<OrderDetail> orderDetails = null;

            _uowStub.Setup(repo => repo.DetailRepo.GetDetailsForOrder(It.IsAny<string>()))
                .Returns(orderDetails);

            var controller = new OrderDetailController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.GetDetailsForOrders(It.IsAny<string>());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetDetailsForOrder_WithReturn_ReturnsOkObject()
        {
            // Arrange
            List<OrderDetail> orderDetails = new()
            {
                GenerateDetails()
            };

            _uowStub.Setup(repo => repo.DetailRepo.GetDetailsForOrder(It.IsAny<string>()))
                .Returns(orderDetails);

            var controller = new OrderDetailController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.GetDetailsForOrders(It.IsAny<string>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        #endregion

        public OrderDetail GenerateDetails()
        {
            return new()
            {
                OrderDetailID = 1,
                OrderNum = "23",
                Price = 23,
                ProductID = 1,
                Quantity = 1,
                TotalPrice = 23
            };
        }

        public OrderDetailDto GenerateDetailsDto()
        {
            return new()
            {
                OrderNum = "23",
                Price = 23,
                Product = "Kofi",
                Quantity = 1,
                TotalPrice = 23
            };
        }
    }
}
