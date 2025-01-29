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

        public RefreshToken GetByToken(string token)
        {
            return _context.RefreshTokens.SingleOrDefault(rt => rt.Token == token);
        }

        public void Add(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
        }

        public void Update(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
        }

        public void Revoke(string token)
        {
            var refreshToken = GetByToken(token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                Update(refreshToken);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
