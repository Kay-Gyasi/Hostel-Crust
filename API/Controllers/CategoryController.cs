namespace API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IDIFactory factory;

        public CategoryController(IUnitOfWork uow, IDIFactory factory)
        {
            this.uow = uow;
            this.factory = factory;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await uow.CategoryRepo.GetCategoriesAsync();

            if(categories == null)
            {
                return NotFound();
            }

            var categoriesDto = from c in categories
                                select new CategoriesDto()
                                {
                                    CategoryID = c.CategoryID,
                                    Title = c.Title
                                };

            return Ok(categoriesDto);
        }


        [HttpPost("PostCategory")]
        public async Task<IActionResult> PostCategory([FromBody] CategoriesDto categoryDto)
        {
            var categoryExists = uow.CategoryRepo.CategoryExists(categoryDto.Title);

            if (categoryExists)
            {
                return BadRequest("Category already exists in database");
            }

            var category = factory.Categories();
            #region Mapping
            category.CategoryID = categoryDto.CategoryID;
            category.Title = categoryDto.Title;
            category.DateAdded = categoryDto.DateAdded;
            #endregion

            uow.CategoryRepo.AddCategory(category);

            await uow.SaveAsync();

            return CreatedAtAction("GetCategories", new { id = category.CategoryID }, categoryDto);
        }


        [HttpPut("PutCategory/{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoriesDto categoryDto)
        {
            var category = await uow.CategoryRepo.GetCategoryById(id);

            if (category is null)
            {
                return BadRequest();
            }

            category.Title = categoryDto.Title;
            category.CategoryID = id;

            await uow.SaveAsync();

            return NoContent();
        }


        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await uow.CategoryRepo.GetCategoryById(id);

            if(category is null)
            {
                return BadRequest("Category does not exist");
            }

            uow.CategoryRepo.DeleteCategory(id);

            await uow.SaveAsync();

            return NoContent();
        }
    }
}
