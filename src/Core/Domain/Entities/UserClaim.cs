using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class UserClaim : BaseEntity
    {
        public int UserId { get; set; }
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;

        // Navigation
        public User User { get; set; }
    }
}
