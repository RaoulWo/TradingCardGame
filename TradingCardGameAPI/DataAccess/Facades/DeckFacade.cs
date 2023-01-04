using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Repositories;
using Npgsql;

namespace DataAccess.Facades;

public class DeckFacade : IDeckFacade
{
    public static DeckFacade Instance
    {
        get
        {
            _instance ??= new DeckFacade(DeckRepository.Instance, UnitOfWork.Instance);

            return _instance;
        }
    }

    private static DeckFacade _instance = null;

    private DeckRepository _deckRepository;
    private IUnitOfWork _unitOfWork;

    public DeckFacade(DeckRepository deckRepository, IUnitOfWork unitOfWork)
    {
        _deckRepository = deckRepository;
        this._unitOfWork = unitOfWork;
    }

    public IEnumerable<DeckEntity> GetAll()
    {
        string sqlStatement = "SELECT * FROM deck";
        IEnumerable<DeckEntity> decks = null;

        try
        {
            decks = _deckRepository.GetAll(sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return decks;
    }

    public DeckEntity GetByGuid(Guid guid)
    {
        string sqlStatement = "SELECT * FROM deck WHERE Id = @Id";
        DeckEntity deck = null;

        try
        {
            deck = _deckRepository.GetByGuid(guid, sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return deck;
    }

    public int Insert(DeckEntity deck)
    {
        string sqlStatement = "INSERT INTO deck (Id, fk_collection_id) VALUES (@Id, @FkCollectionId)";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _deckRepository.Insert(deck, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }

    public int Update(DeckEntity collection)
    {
        string sqlStatement = "UPDATE deck SET fk_collection_id = @FkCollectionId WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _deckRepository.Update(collection, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }

    public int DeleteByGuid(Guid guid)
    {
        string sqlStatement = "DELETE FROM deck WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _deckRepository.DeleteByGuid(guid, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }
}