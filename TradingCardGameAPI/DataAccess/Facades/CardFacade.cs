using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Repositories;
using Npgsql;

namespace DataAccess.Facades;

public class CardFacade : ICardFacade
{
    public static CardFacade Instance
    {
        get
        {
            _instance ??= new CardFacade(CardRepository.Instance, UnitOfWork.Instance);

            return _instance;
        }

    }

    private static CardFacade _instance = null;

    private CardRepository _cardRepository;
    private IUnitOfWork _unitOfWork;

    public CardFacade(CardRepository cardRepository, IUnitOfWork unitOfWork)
    {
        _cardRepository = cardRepository;
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<CardEntity> GetAll()
    {
        string sqlStatement = "SELECT * FROM card";
        IEnumerable<CardEntity> cards = null;

        try
        {
            cards = _cardRepository.GetAll(sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return cards;
    }

    public CardEntity GetByGuid(Guid guid)
    {
        string sqlStatement = "SELECT * FROM card WHERE Id = @Id";
        CardEntity card = null;

        try
        {
            card = _cardRepository.GetByGuid(guid, sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return card;
    }

    public int Insert(CardEntity card)
    {
        string sqlStatement = "INSERT INTO card (Id, Name, Type, Element, Damage, Health, Cost) VALUES (@Id, @Name, @Type, @Element, @Damage, @Health, @Cost)";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _cardRepository.Insert(card, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }

    public int Update(CardEntity card)
    {
        string sqlStatement = "UPDATE card SET Name = @Name, Type = @Type, Element = @Element, Damage = @Damage, Health = @Health, Cost = @Cost WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _cardRepository.Update(card, sqlStatement, transaction);
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
        string sqlStatement = "DELETE FROM card WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _cardRepository.DeleteByGuid(guid, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }
}