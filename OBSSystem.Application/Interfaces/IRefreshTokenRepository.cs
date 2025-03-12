using OBSSystem.Core.Entities;

namespace OBSSystem.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        RefreshToken GetByToken(string token);
        void Add(RefreshToken refreshToken);
        void Update(RefreshToken refreshToken);
        void Revoke(string token);
        void SaveChanges();
    }
}