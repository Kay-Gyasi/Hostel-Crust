using Data_Layer.Models;

namespace API.DTOs
{
    public class OrderDto
    {
        public int OrderID { get; set; }

        public string Customer { get; set; }

        public bool? isFulfilled { get; set; } = false;

        public DateTime? DateOrdered { get; set; }

        public string OrderNum { get; set; }

        public bool? isDelivery { get; set; } = false;

        public string? AdditionalInfo { get; set; }

        public string DeliveryLocation { get; set; }
    }
}
