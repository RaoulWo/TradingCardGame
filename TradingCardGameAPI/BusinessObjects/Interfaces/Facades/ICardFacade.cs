using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Facades;

public interface ICardFacade
{
    IEnumerable<CardEntity> GetAll();
    CardEntity GetByGuid(Guid guid);
    int Insert(CardEntity cardEntity);
    int Update(CardEntity cardEntity);
    int DeleteByGuid(Guid guid);
}