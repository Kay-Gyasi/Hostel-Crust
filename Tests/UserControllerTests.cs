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
    public class UserControllerTests
    {
        private readonly Mock<IUnitOfWork> _uowStub = new();

        #region GetUsers
        [Fact]
        public async Task GetUsers_UsersExist_ReturnsOkObject()
        {
            // Arrange
            IEnumerable<Users> users = new[]
            {
                GenerateUser(),
                GenerateUser()
            };

            _uowStub.Setup(repo => repo.UserRepo.GetUsersAsync()).ReturnsAsync(users);

            var controller = new UserController(_uowStub.Object);

            // Act
            var result = await controller.GetUsers();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUsers_UsersNotExist_ReturnsNotFound()
        {
            // Arrange
            IEnumerable<Users> users = null;

            _uowStub.Setup(repo => repo.UserRepo.GetUsersAsync()).ReturnsAsync(users);

            var controller = new UserController(_uowStub.Object);

            // Act
            var result = await controller.GetUsers();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion


        #region DeleteUser
        [Fact]
        public async Task DeleteUser_UserExists_ReturnsNoContent()
        {
            // Arrange
            Users user = GenerateUser();

            _uowStub.Setup(repo => repo.UserRepo.GetUsersById(It.IsAny<int>()))
                .ReturnsAsync(user);

            var controller = new UserController(_uowStub.Object);

            // Act
            var result = await controller.DeleteUser(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteUser_UserNotExists_ReturnsBadRequest()
        {
            // Arrange
            Users user = null;

            _uowStub.Setup(repo => repo.UserRepo.GetUsersById(It.IsAny<int>()))
                .ReturnsAsync(user);

            var controller = new UserController(_uowStub.Object);

            // Act
            var result = await controller.DeleteUser(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
        #endregion


        #region AddUser
        [Fact]
        public async Task AddUser_UserNotExists_ReturnsOkObject()
        {
            // Arrange
            AccountsDto userToAdd = GenerateAccountsDto();

            _uowStub.Setup(repo => repo.UserRepo.UserAlreadyExists(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var controller = new UserController(_uowStub.Object);

            // Act
            var result = await controller.AddUser(userToAdd);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AddUser_UserExists_ReturnsBadRequest()
        {
            // Arrange
            AccountsDto userToAdd = GenerateAccountsDto();

            _uowStub.Setup(repo => repo.UserRepo.UserAlreadyExists(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new UserController(_uowStub.Object);

            // Act
            var result = await controller.AddUser(userToAdd);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        public Users GenerateUser()
        {
            return new()
            {
                FirstName = "Kofi",
                LastName = "Gyasi",
                Email = "kay",
                Password = new byte[8],
                PasswordKey = new byte[8],
                DateJoined = DateTime.Now,
                Phone = "0557",
                CustomerID = 3
            };
        }

        public UsersDto GenerateUserDto()
        {
            return new()
            {
                FirstName = "Kofi",
                LastName = "Gyasi",
                Email = "kay",
                Password = new byte[8],
                Phone = "0557",
                CustomerID = 3
            };
        }

        public AccountsDto GenerateAccountsDto()
        {
            return new()
            {
                FirstName = "Kofi",
                LastName = "Gyasi",
                Email = "kay",
                Password = "Pass",
                Phone = "0557",
                CustomerID = 3
            };
        }
    }
}
