using API.Controllers;
using API.DTOs;
using API.Interfaces;
using Data_Layer.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class OrderDetailControllerTests
    {
        private readonly Mock<IUnitOfWork> _uowStub = new();

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

            var controller = new OrderDetailController(_uowStub.Object);

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

            var controller = new OrderDetailController(_uowStub.Object);

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

            var controller = new OrderDetailController(_uowStub.Object);

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

            var controller = new OrderDetailController(_uowStub.Object);

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

            var controller = new OrderDetailController(_uowStub.Object);

            // Act
            var result = await controller.PostOrderDetail(GenerateDetailsDto());

            // Assert
            var postedDetail = (result as CreatedAtActionResult).Value as OrderDetailDto;

            detailToPost.Should().BeEquivalentTo(postedDetail,
                options => options.ComparingByMembers<OrderDetailDto>()
                .ExcludingMissingMembers());
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
