namespace API.Factory
{
    public class DIFactory : IDIFactory
    {
        public Categories Categories()
        {
            return new();
        }

        public CategoriesDto CategoriesDto()
        {
            return new();
        }

        public LoginResDto LoginResDto()
        {
            return new();
        }

        public OrderDetail OrderDetail()
        {
            return new();
        }

        public OrderDto OrderDto()
        {
            return new();
        }

        public Orders Orders()
        {
            return new();
        }

        public Products Products()
        {
            return new();
        }
    }
}
