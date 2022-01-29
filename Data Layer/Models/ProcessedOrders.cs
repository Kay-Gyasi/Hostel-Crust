namespace Data_Layer.Models
{
    public class ProcessedOrders
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("User")]
        public int CustomerID { get; set; }
        public Users User { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string OrderNum { get; set; }

        public bool? isFulfilled { get; set; } 

        public bool? isDelivery { get; set; } 

        [Column(TypeName = "varchar(200)")]
        [DataType(DataType.MultilineText)]
        public string? AdditionalInfo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? DeliveryLocation { get; set; }

        public DateTime? DateOrdered { get; set; } 
    }
}
