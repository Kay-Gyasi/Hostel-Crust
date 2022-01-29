namespace Tests
{
    public class CategoryControllerTests
    {
        private readonly Mock<IUnitOfWork> _uowStub = new();

        #region GetCategories
        [Fact]
        public async Task GetCategories_WithValues_ReturnsOkObjectResult()
        {
            // Arrange
            var categories = new[] { GenerateCategory(), GenerateCategory(), GenerateCategory(), GenerateCategory() };

            _uowStub.Setup(repo => repo.CategoryRepo.GetCategoriesAsync()).ReturnsAsync(categories);

            var controller = new CategoryController(_uowStub.Object);

            // Act
            var result = await controller.GetCategories();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCategories_WithNullValue_ReturnsNotFoundResult()
        {
            // Arrange
            IEnumerable<Categories> categories = null;

            _uowStub.Setup(repo => repo.CategoryRepo.GetCategoriesAsync()).ReturnsAsync(categories);

            var controller = new CategoryController(_uowStub.Object);

            // Act
            var result = await controller.GetCategories();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region PostCategory
        [Fact]
        public async Task PostCategory_WithExistingCategory_ReturnsBadRequest()
        {
            // Arrange
            var categoryToPost = GenerateCategoryDto();
            _uowStub.Setup(repo => repo.CategoryRepo.CategoryExists(It.IsAny<string>()))
                .Returns(true);

            var controller = new CategoryController(_uowStub.Object);

            // Act
            var result = await controller.PostCategory(categoryToPost);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PostCategory_WithUnexistingCategory_ReturnsBadRequest()
        {
            // Arrange
            var categoryToPost = new CategoriesDto();

            _uowStub.Setup(repo => repo.CategoryRepo.CategoryExists(It.IsAny<string>()))
                .Returns(false);

            var controller = new CategoryController(_uowStub.Object);

            // Act
            var result = await controller.PostCategory(categoryToPost);

            // Assert
            var createdCategory = (result as CreatedAtActionResult).Value as CategoriesDto;

            categoryToPost.Should().BeEquivalentTo(createdCategory,
                options => options.ComparingByMembers<CategoriesDto>()
                .ExcludingMissingMembers());

            createdCategory.DateAdded.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1000));
        }
        #endregion

        #region PutCategory
        [Fact]
        public async Task PutCategory_WithUnexistingCategory_ReturnsBadRequest()
        {
            // Arrange
            Categories categories = null;

            _uowStub.Setup(repo => repo.CategoryRepo.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(categories);

            var controller = new CategoryController(_uowStub.Object);

            // Act
            var result = await controller.PutCategory(5, GenerateCategoryDto());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutCategory_WithExistingCategory_ReturnsNoContent()
        {
            // Arrange
            var categoryDto = GenerateCategoryDto();

            _uowStub.Setup(repo => repo.CategoryRepo.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(GenerateCategory());

            var controller = new CategoryController(_uowStub.Object);

            // Act
            var result = await controller.PutCategory(5, categoryDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion


        #region DeleteCategory
        [Fact]
        public async Task DeleteCategory_WithExistingCategory_ReturnsNoContent()
        {
            // Arrange
            Categories category = GenerateCategory();

            _uowStub.Setup(repo => repo.CategoryRepo.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(category);

            var controller = new CategoryController(_uowStub.Object);

            // Act
            var result = await controller.DeleteCategory(category.CategoryID);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_WithUnexistingCategory_ReturnsNoContent()
        {
            // Arrange
            var category = GenerateCategory();

            _uowStub.Setup(repo => repo.CategoryRepo.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync((Categories)null);

            var controller = new CategoryController(_uowStub.Object);

            // Act
            var result = await controller.DeleteCategory(category.CategoryID);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion


        public Categories GenerateCategory()
        {
            return new Categories()
            {
                CategoryID = 1,
                Title = "Fun"
            };
        }

        public CategoriesDto GenerateCategoryDto()
        {
            return new CategoriesDto()
            {
                Title = "Fun"
            };
        }
    }
}
