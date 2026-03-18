using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer"; // Customer, Admin
        public bool IsActive { get; set; } = true;
        
        // Navigation
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
