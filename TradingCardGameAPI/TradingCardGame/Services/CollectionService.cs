using BusinessObjects.Entities;
using DataAccess.Facades;

namespace BusinessLogic.Services;

public class CollectionService
{
    public static CollectionService Instance
    {
        get
        {
            _instance ??= new CollectionService(CollectionFacade.Instance);

            return _instance;
        }
    }

    private static CollectionService _instance = null;

    private CollectionFacade _collectionFacade;

    public CollectionService(CollectionFacade collectionFacade)
    {
        _collectionFacade = collectionFacade;
    }

    public void StorePackageInCollection(Guid fkPlayerId, List<CardEntity> package)
    {
        foreach (var card in package)
        {
            // Create collection entity
            var collectionEntity = new CollectionEntity()
            {
                FkPlayerId = fkPlayerId,
                FkCardId = (Guid)card.Id
            };

            // Store the collection entity in the db
            _collectionFacade.Insert(collectionEntity);
        }
    }
}