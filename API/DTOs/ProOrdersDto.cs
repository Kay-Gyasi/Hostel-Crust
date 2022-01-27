using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProOrdersDto
    {
        public int OrderID { get; set; }

        public string Customer { get; set; }

        public string OrderNum { get; set; }

        public bool? isFulfilled { get; set; } = false;

        public bool? isDelivery { get; set; } = false;

        public string? AdditionalInfo { get; set; }

        public string? DeliveryLocation { get; set; }

        public DateTime? DateOrdered { get; set; } = DateTime.Now;
    }
}
