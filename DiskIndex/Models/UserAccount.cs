namespace DiskIndex.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = "Guest"; // or "Admin"
        public bool NSFWTag { get; set; } = false;
    }
}
