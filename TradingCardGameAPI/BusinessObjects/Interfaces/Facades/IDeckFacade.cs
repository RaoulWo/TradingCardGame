using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Facades;

public interface IDeckFacade
{
    IEnumerable<DeckEntity> GetAll();
    DeckEntity GetByGuid(Guid guid);
    int Insert(DeckEntity deckEntity);
    int Update(DeckEntity deckEntity);
    int DeleteByGuid(Guid guid);
}