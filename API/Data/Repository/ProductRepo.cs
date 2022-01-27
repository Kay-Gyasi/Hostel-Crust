using API.Interfaces;
using Data_Layer.Data_Context;
using Data_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly HostelContext db;

        public ProductRepo(HostelContext db)
        {
            this.db = db;
        }

        #region AddProduct
        public void AddProduct(Products product)
        {
           db.products.Add(product);
        }
        #endregion

        #region DeleteProduct
        public void DeleteProduct(int id)
        {
            var product = db.products.Find(id);

            if(product != null)
            {
                db.products.Remove(product);
            }
        }
        #endregion

        #region GetProductById
        public async Task<Products> GetProductById(int id)
        {
            var product = await db.products.FindAsync(id);
            return product;
        }
        #endregion

        #region GetProductsAsync
        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            return await db.products.ToListAsync();
        }
        #endregion

        public int GetCategoryId(string name)
        {
            return db.categories.FirstOrDefault(c => c.Title == name).CategoryID;
        }

        public string GetCategoryName(int id)
        {
            return db.categories.FirstOrDefault(a => a.CategoryID == id).Title;
        }

        public bool ProductExists(string title)
        {
            return db.products.Where(x => x.Name == title).Any();
        }
    }
}
