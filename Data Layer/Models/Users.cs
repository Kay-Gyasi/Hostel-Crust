using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Models
{
    public class Users
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Enter your first name")]
        [DataType(DataType.Text, ErrorMessage = "Invalid name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter your last name"), MaxLength(50)]
        [DataType(DataType.Text, ErrorMessage = "Invalid name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email must be provided")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        [Column(TypeName = "varchar(55)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Provide a password")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password")]
        public byte[] Password { get; set; }

        [Required(ErrorMessage = "Provide a password key")]
        public byte[] PasswordKey { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Column(TypeName = "varchar(15)")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        public DateTime DateJoined { get; set; } = DateTime.Now;

        [MaxLength(100)]
        [DataType(DataType.MultilineText, ErrorMessage = "Invalid Address")]
        public string? Address { get; set; }



        public ICollection<Orders> Orders { get; set; }

    }
}
