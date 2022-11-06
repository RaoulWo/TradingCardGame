using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Repositories;
using Npgsql;

namespace DataAccess.Facades;

public class PlayerFacade : IPlayerFacade
{
    public static PlayerFacade Instance
    {
        get
        {
            _instance ??= new PlayerFacade(PlayerRepository.Instance, UnitOfWork.Instance);

            return _instance;
        }
    }

    private static PlayerFacade _instance = null;

    private PlayerRepository _playerRepository;
    private IUnitOfWork _unitOfWork;

    public PlayerFacade(PlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<PlayerEntity> GetAll()
    {
        string sqlStatement = "SELECT * FROM player";
        IEnumerable<PlayerEntity> players = null;

        try
        {
            players = _playerRepository.GetAll(sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return players;
    }

    public PlayerEntity GetByGuid(Guid guid)
    {
        string sqlStatement = "SELECT * FROM player WHERE Id = @Id";
        PlayerEntity player = null;

        try
        {
            player = _playerRepository.GetByGuid(guid, sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return player;
    }

    public int Insert(PlayerEntity player)
    {
        string sqlStatement = "INSERT INTO player (Name, Password, Coins) VALUES (@Name, @Password, @Coins)";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _playerRepository.Insert(player, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }

    public int Update(PlayerEntity player)
    {
        string sqlStatement = "UPDATE player SET Name = @Name, Password = @Password, Coins = @Coins WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _playerRepository.Update(player, sqlStatement, transaction);
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
        string sqlStatement = "DELETE FROM player WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _playerRepository.DeleteByGuid(guid, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }
}