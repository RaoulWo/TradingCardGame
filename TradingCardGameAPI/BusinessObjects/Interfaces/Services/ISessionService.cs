using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Services;

public interface ISessionService
{
    void CreateSession(SessionEntity session);
    void DestroySessionById(Guid id);
    void DestroySessionByFkPlayerId(Guid fkPlayerId);
    bool CheckSessionByFkPlayerId(Guid fkPlayerId);
    string Hash(string password);
    bool VerifyPassword(string userEnteredPwd, string storedPwd);
}