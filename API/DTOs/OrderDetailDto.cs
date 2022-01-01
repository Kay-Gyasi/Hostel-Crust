namespace API.DTOs
{
    public class OrderDetailDto
    {
        public string OrderNum { get; set; }

        public string Product { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
