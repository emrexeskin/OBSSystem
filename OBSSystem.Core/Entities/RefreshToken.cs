using System;

namespace OBSSystem.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; } // Primary Key
        public string Token { get; set; } // Benzersiz Refresh Token
        public int UserId { get; set; } // Kullanıcıyla ilişkilendirme
        public DateTime ExpiryDate { get; set; } // Token'ın geçerlilik süresi
        public bool IsRevoked { get; set; } // Token'ın iptal edilip edilmediği
        public bool IsUsed { get; set; } // Token kullanıldı mı (Token Rotation için)

        // Navigation Property
        public User User { get; set; } // Token'ın ait olduğu kullanıcı
    }
}
