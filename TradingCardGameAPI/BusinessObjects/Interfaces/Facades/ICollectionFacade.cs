using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Facades;

public interface ICollectionFacade
{
    IEnumerable<CollectionEntity> GetAll();
    CollectionEntity GetByGuid(Guid guid);
    int Insert(CollectionEntity collection);
    int Update(CollectionEntity collection);
    int DeleteByGuid(Guid guid);
}