namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Nama { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;


        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }

        public int Status { get; set; }
    }
}
