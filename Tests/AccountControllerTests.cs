using API.Controllers;
using API.Data.Repository;
using API.DTOs;
using API.Interfaces;
using Data_Layer.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<IUnitOfWork> _uowStub = new();
        private readonly Mock<IConfiguration> configurationStub = new();
        private Random rand = new();

        [Fact]
        public async Task Login_WithUnexistingUser_ShouldReturnUnauthorized()
        {
            // Arrange
            Users user = null;

            _uowStub.Setup(repo => repo.UserRepo.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            var controller = new AccountController(_uowStub.Object, configurationStub.Object);

            // Act
            var result = await controller.Login(GenerateLoginRequest());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }


        [Fact]
        public async Task Login_WithExistingUser_ShouldReturnOkObjectResult()
        {
            // Arrange
            Users user = GenerateUser();

            _uowStub.Setup(repo => repo.UserRepo.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            var controller = new AccountController(_uowStub.Object, configurationStub.Object);

            // Act
            var result = await controller.Login(GenerateLoginRequest());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }


        public LoginReqDto GenerateLoginRequest()
        {
            return new()
            {
                Username = "Kofi Gyasi",
                Password = "hahahhahahahah"
            };
        }

        public UsersDto GenerateUserDto()
        {
            return new()
            {
                FirstName = "Kofi",
                LastName = "Gyasi",
                Email = "kaygyasi",
                Address = "Anaji",
                CustomerID = 1,
                Phone = "0557833216",
                Password = new byte[10]
            };
        }

        public Users GenerateUser()
        {
            return new()
            {
                FirstName = "Kofi",
                LastName = "Gyasi",
                Email = "kaygyasi",
                Address = "Anaji",
                CustomerID = 1,
                Phone = "0557833216",
                Password = new byte[10],
                DateJoined = DateTime.Now
            };
        }
    }
}