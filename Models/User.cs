namespace TaskFlow.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // We only store hashed password — never plain text
        public string PasswordHash { get; set; } = string.Empty;

        // Role could be Admin, Manager, Member
        public string Role { get; set; } = "Member";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Optional: track verification and status
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;
    }
}
