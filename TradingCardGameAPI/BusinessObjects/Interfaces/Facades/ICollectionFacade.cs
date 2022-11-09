using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Facades;

public interface ICollectionFacade
{
    IEnumerable<CollectionEntity> GetAll();
    CollectionEntity GetByGuid(Guid guid);
    int Insert(CollectionEntity collectionEntity);
    int Update(CollectionEntity collectionEntity);
    int DeleteByGuid(Guid guid);
}