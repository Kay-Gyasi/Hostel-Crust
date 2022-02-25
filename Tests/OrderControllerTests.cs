namespace Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<IUnitOfWork> _uowStub = new Mock<IUnitOfWork>();
        private readonly Mock<IDIFactory> factoryStub = new();

        #region GetOrders
        [Fact]
        public async Task GetOrders_WithNullReturn_ReturnsNotFound()
        {
            // Arrange
            IEnumerable<Orders> orders = null;

            _uowStub.Setup(repo => repo.OrderRepo.GetOrdersAsync()).ReturnsAsync(orders);

            var controller = new OrderController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.GetOrders();

            // Assert
            result.Should().BeOfType<NotFoundResult>();

        }

        [Fact]
        public async Task GetOrders_WithReturn_ReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<Orders> orders = new[]
            {
                GenerateOrder(),
                GenerateOrder()
            };

            _uowStub.Setup(repo => repo.OrderRepo.GetOrdersAsync()).ReturnsAsync(orders);

            var controller = new OrderController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.GetOrders();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

        }
        #endregion


        #region PostOrder
        [Fact]
        public async Task PostOrder_ReturnsCreatedResult()
        {
            // Arrange
            OrderDto orderToPost = GenerateOrderDto();

            _uowStub.Setup(repo => repo.OrderRepo.GetCustomerId(It.IsAny<string>()))
                .Returns(It.IsAny<int>());
            factoryStub.Setup(x => x.Orders()).Returns(GenerateOrder());

            var controller = new OrderController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.PostOrder(orderToPost);

            // Assert
            var postedOrder = (result as CreatedAtActionResult).Value as OrderDto;

            orderToPost.Should().BeEquivalentTo(postedOrder,
                options => options.ComparingByMembers<OrderDto>()
                .ExcludingMissingMembers());

            postedOrder.isFulfilled.Should().HaveValue();
            postedOrder.DateOrdered.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1000));
            postedOrder.isDelivery.Should().NotBeNull();
        }
        #endregion


        #region PutOrder
        [Fact]
        public async Task PutOrder_WithUnexistingOrder_ReturnsBadRequest()
        {
            // Arrange
            Orders orderToPut = null;

            _uowStub.Setup(repo => repo.OrderRepo.GetOrderById(It.IsAny<int>()))
                .ReturnsAsync(orderToPut);

            var controller = new OrderController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.PutOrder(It.IsAny<int>(), GenerateOrderDto());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutOrder_WithExistingOrder_ReturnsNoContent()
        {
            // Arrange
            Orders orderToPut = GenerateOrder();

            _uowStub.Setup(repo => repo.OrderRepo.GetOrderById(It.IsAny<int>()))
                .ReturnsAsync(orderToPut);

            var controller = new OrderController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.PutOrder(It.IsAny<int>(), GenerateOrderDto());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion


        #region DeleteOrder
        [Fact]
        public async Task DeleteOrder_WithExistingOrder_ReturnsNoContent()
        {
            // Arrange
            Orders orderToDelete = GenerateOrder();

            _uowStub.Setup(repo => repo.OrderRepo.GetOrderById(It.IsAny<int>()))
                .ReturnsAsync(orderToDelete);

            var controller = new OrderController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.DeleteOrder(orderToDelete.OrderID);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_WithUnexistingOrder_ReturnsBadRequestObject()
        {
            // Arrange
            Orders orderToDelete = null;

            _uowStub.Setup(repo => repo.OrderRepo.GetOrderById(It.IsAny<int>()))
                .ReturnsAsync(orderToDelete);

            var controller = new OrderController(_uowStub.Object, factoryStub.Object);

            // Act
            var result = await controller.DeleteOrder(It.IsAny<int>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion


        public Orders GenerateOrder()
        {
            return new()
            {
                CustomerID = 0,
                DateOrdered = DateTime.Now,
                OrderID = 0,
                OrderNum = "23"
            };
        }

        public OrderDto GenerateOrderDto()
        {
            return new()
            {
                DateOrdered = DateTime.Now,
                Customer = "Kofi",
                OrderNum = " 234",
                OrderID = 0,
                DeliveryLocation = "Kaspa"
            };
        }
    }
}
