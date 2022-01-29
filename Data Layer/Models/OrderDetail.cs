namespace Data_Layer.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string OrderNum { get; set; }

        [ForeignKey("Products")]
        public int ProductID { get; set; }
        public Products Products { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal TotalPrice { get; set; }
    }
}
