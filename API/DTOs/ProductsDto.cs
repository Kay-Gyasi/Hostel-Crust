namespace API.DTOs
{
    public class ProductsDto
    {
        public int ProductID { get; set; }

        public string CategoryName { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public bool? isAvailable { get; set; } = true;
    }
}
