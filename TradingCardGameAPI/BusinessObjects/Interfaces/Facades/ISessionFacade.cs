using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Facades;

public interface ISessionFacade
{
    IEnumerable<SessionEntity> GetAll();
    SessionEntity GetByGuid(Guid guid);
    int Insert(SessionEntity session);
    int Update(SessionEntity session);
    int DeleteByGuid(Guid guid);
}