using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public ICollection<Products>? Products { get; set; }
    }
}
