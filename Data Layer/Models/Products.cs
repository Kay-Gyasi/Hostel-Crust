namespace Data_Layer.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }

        [ForeignKey("Categories")]
        [Required(ErrorMessage = "Provide category")]
        public int CategoryID { get; set; }
        public Categories Categories { get; set; }

        [Required(ErrorMessage = "Provide name of product")]
        [Column(TypeName = "varchar(55)")]
        [DataType(DataType.Text, ErrorMessage = "Invalid title")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Provide product price")]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency, ErrorMessage = "Invalid price")]
        public decimal Price { get; set; }

        public bool? isAvailable { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
