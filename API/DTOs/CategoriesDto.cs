namespace API.DTOs
{
    public class CategoriesDto
    {
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public DateTime? DateAdded { get; set; } = DateTime.Now;

    }
}
