using API.Interfaces;
using Data_Layer.Data_Context;
using Data_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly HostelContext db;

        public CategoryRepo(HostelContext db)
        {
            this.db = db;
        }

        #region AddCategory
        public void AddCategory(Categories category)
        {
            db.categories.Add(category);
        }

        public bool CategoryExists(string name)
        {
            return db.categories.Any(x => x.Title == name);
        }
        #endregion

        #region DeleteCategory
        public void DeleteCategory(int id)
        {
            var category = db.categories.Find(id);

            if(category != null)
            {
                db.categories.Remove(category);
            }
        }
        #endregion

        #region GetCategoriesAsync
        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            return await db.categories.ToListAsync();
        }
        #endregion

        #region GetCategoryById
        public async Task<Categories> GetCategoryById(int id)
        {
            return await db.categories.FindAsync(id);
        }

        public Task<Categories> GetCategoryByName(string name)
        {
            var category = db.categories.FirstOrDefaultAsync(x => x.Title == name);

            if (category == null)
            {
                throw new ArgumentException(name);
            }
            else
            {
                return category;
            }
        }
        #endregion
    }
}
