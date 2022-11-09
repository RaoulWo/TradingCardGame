using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Repositories;
using Npgsql;

namespace DataAccess.Facades;

public class CollectionFacade : ICollectionFacade
{
    public static CollectionFacade Instance
    {
        get
        {
            _instance ??= new CollectionFacade(CollectionRepository.Instance, UnitOfWork.Instance);

            return _instance;
        }
    }

    private static CollectionFacade _instance = null;

    private CollectionRepository _collectionRepository;
    private IUnitOfWork _unitOfWork;

    public CollectionFacade(CollectionRepository collectionRepository, IUnitOfWork unitOfWork)
    {
        _collectionRepository = collectionRepository;
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<CollectionEntity> GetAll()
    {
        string sqlStatement = "SELECT * FROM collection";
        IEnumerable<CollectionEntity> collections = null;

        try
        {
            collections = _collectionRepository.GetAll(sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return collections;
    }

    public CollectionEntity GetByGuid(Guid guid)
    {
        string sqlStatement = "SELECT * FROM collection WHERE Id = @Id";
        CollectionEntity collection = null;

        try
        {
            collection = _collectionRepository.GetByGuid(guid, sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return collection;
    }

    public int Insert(CollectionEntity collection)
    {
        string sqlStatement = "INSERT INTO collection (Id, fk_player_id, fk_card_id) VALUES (@Id, @PlayerId, @CardId)";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _collectionRepository.Insert(collection, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }

    public int Update(CollectionEntity collection)
    {
        string sqlStatement = "UPDATE collection SET fk_player_id = @PlayerId, fk_card_id = @CardId WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _collectionRepository.Update(collection, sqlStatement, transaction);
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
        string sqlStatement = "DELETE FROM collection WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _collectionRepository.DeleteByGuid(guid, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }
}