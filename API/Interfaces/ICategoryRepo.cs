namespace API.Interfaces
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<Categories>> GetCategoriesAsync();

        void AddCategory(Categories category);  

        void DeleteCategory(int id);

        Task<Categories> GetCategoryById(int id);

        Task<Categories> GetCategoryByName(string name);

        bool CategoryExists(string name);
    }
}
