using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Services;

public interface ISessionService
{
    void DestroySession(SessionEntity sessionEntity);
    void StoreSession(SessionEntity sessionEntity);
}