using System.Linq;
using Microsoft.EntityFrameworkCore;
using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;

namespace OBSSystem.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly OBSContext _context;

        public RefreshTokenRepository(OBSContext context)
        {
            _context = context;
        }

        // Token ile refresh token getir (senkron)
        public RefreshToken GetByToken(string token)
        {
            return _context.RefreshTokens.SingleOrDefault(rt => rt.Token == token);
        }

        // Refresh token ekle (senkron)
        public void Add(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();
        }

        // Refresh token güncelle (senkron)
        public void Update(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            _context.SaveChanges();
        }

        // Token'ı iptal et (senkron)
        public void Revoke(string token)
        {
            var refreshToken = GetByToken(token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                Update(refreshToken);
            }
        }

        // Değişiklikleri kaydet (senkron)
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}