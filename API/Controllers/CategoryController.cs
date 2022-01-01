using API.DTOs;
using API.Interfaces;
using Data_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var category = new Categories
            {
                CategoryID = categoryDto.CategoryID,
                Title = categoryDto.Title
            };

            uow.CategoryRepo.AddCategory(category);

            await uow.SaveAsync();

            return CreatedAtAction("GetCategories", new { id = category.CategoryID }, categoryDto);
        }


        [HttpPut("PutCategory/{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoriesDto categoryDto)
        {
            if(id != categoryDto.CategoryID)
            {
                return BadRequest();
            }

            var category = await uow.CategoryRepo.GetCategoryById(id);

            category.Title = categoryDto.Title;
            category.CategoryID = id;

            await uow.SaveAsync();

            return StatusCode(200);
        }


        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            uow.CategoryRepo.DeleteCategory(id);

            await uow.SaveAsync();

            return Ok(id);
        }
    }
}
