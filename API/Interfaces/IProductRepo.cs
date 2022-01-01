using Data_Layer.Models;

namespace API.Interfaces
{
    public interface IProductRepo
    {
        Task<IEnumerable<Products>> GetProductsAsync();

        void AddProduct(Products product);

        void DeleteProduct(int id);

        Task<Products> GetProductById(int id);

        int GetCategoryId(string name);

        string GetCategoryName(int id);
    }
}
