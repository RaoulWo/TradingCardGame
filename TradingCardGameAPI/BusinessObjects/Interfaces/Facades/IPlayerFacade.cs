using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Facades;

public interface IPlayerFacade
{
    IEnumerable<PlayerEntity> GetAll();
    PlayerEntity GetByGuid(Guid guid);
    int Insert(PlayerEntity playerEntity);
    int Update(PlayerEntity playerEntity);
    int DeleteByGuid(Guid guid);
}