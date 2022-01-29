namespace Data_Layer.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(30)]
        [DataType(DataType.Text, ErrorMessage = "Invalid title")]
        public string Title { get; set; }

        public DateTime? DateAdded { get; set; }

        public ICollection<Products>? Products { get; set; }
    }
}
