using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Facades;

public interface IDeckFacade
{
    IEnumerable<DeckEntity> GetAll();
    DeckEntity GetByGuid(Guid guid);
    int Insert(DeckEntity deck);
    int Update(DeckEntity deck);
    int DeleteByGuid(Guid guid);
}