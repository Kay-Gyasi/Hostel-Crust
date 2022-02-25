
namespace API.Factory
{
    public interface IDIFactory
    {
        LoginResDto LoginResDto();
        CategoriesDto CategoriesDto();
        Categories Categories();
        Orders Orders();
        OrderDto OrderDto();
        OrderDetail OrderDetail();
        Products Products();
    }
}