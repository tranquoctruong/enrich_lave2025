using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? RevokedDate { get; set; } // Nếu token bị thu hồi
        public string? ReplacedByToken { get; set; } // Token thay thế (nếu có)

        // FK liên kết đến User
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
