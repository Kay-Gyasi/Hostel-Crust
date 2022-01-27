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
    public class ProcessedOrderControllerTests
    {
        private readonly Mock<IUnitOfWork> _uowStub = new();

        #region GetProOrder
        [Fact]
        public async Task ProcessedOrderController_WithNullValue_ReturnsNotFound()
        {
            // Arrange
            // Arrange
            IEnumerable<ProcessedOrders> orders = null;

            _uowStub.Setup(repo => repo.ProOrdersRepo.GetProcessedOrders()).ReturnsAsync(orders);

            var controller = new ProOrdersController(_uowStub.Object);

            // Act
            var result = await controller.GetProOrders();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ProcessedOrderController_WithValues_ReturnsOkObject()
        {
            // Arrange
            IEnumerable<ProcessedOrders> orders = new[]
            {
                GenerateProcessedOrders(),
                GenerateProcessedOrders()
            };

            _uowStub.Setup(repo => repo.ProOrdersRepo.GetProcessedOrders()).ReturnsAsync(orders);

            var controller = new ProOrdersController(_uowStub.Object);

            // Act
            var result = await controller.GetProOrders();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

        }
        #endregion

        public ProcessedOrders GenerateProcessedOrders()
        {
            return new()
            {
                CustomerID = 0,
                DateOrdered = DateTime.Now,
                OrderID = 0,
                OrderNum = "23"
            };
        }

        public ProOrdersDto GenerateProcessedOrdersDto()
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
