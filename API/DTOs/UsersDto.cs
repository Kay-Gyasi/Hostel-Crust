namespace API.DTOs
{
    public class UsersDto
    {
        public int CustomerID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public byte[] Password { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
