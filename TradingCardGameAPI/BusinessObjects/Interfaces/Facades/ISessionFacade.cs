using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Facades;

public interface ISessionFacade
{
    IEnumerable<SessionEntity> GetAll();
    SessionEntity GetByGuid(Guid guid);
    int Insert(SessionEntity sessionEntity);
    int Update(SessionEntity sessionEntity);
    int DeleteByGuid(Guid guid);
}