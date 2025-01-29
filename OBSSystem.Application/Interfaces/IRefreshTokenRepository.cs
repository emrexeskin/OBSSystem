using OBSSystem.Core.Entities;

public interface IRefreshTokenRepository
{
    RefreshToken GetByToken(string token);
    void Add(RefreshToken refreshToken);
    void Update(RefreshToken refreshToken);
    void Revoke(string token);
    void SaveChanges();
}
