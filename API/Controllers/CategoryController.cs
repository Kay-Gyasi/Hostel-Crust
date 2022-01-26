using API.DTOs;
using API.Interfaces;
using Data_Layer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWork uow;

        public CategoryController(IUnitOfWork uow)
        {
            this.uow = uow;
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

            var category = new Categories
            {
                CategoryID = categoryDto.CategoryID,
                Title = categoryDto.Title,
                DateAdded = categoryDto.DateAdded
            };

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
