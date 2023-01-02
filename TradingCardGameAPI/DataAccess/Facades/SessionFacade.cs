using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Repositories;
using Npgsql;

namespace DataAccess.Facades;

public class SessionFacade : ISessionFacade
{
    public static SessionFacade Instance
    {
        get
        {
            _instance ??= new SessionFacade(SessionRepository.Instance, UnitOfWork.Instance);

            return _instance;
        }
    }

    private static SessionFacade _instance = null;

    private SessionRepository _sessionRepository;
    private IUnitOfWork _unitOfWork;

    public SessionFacade(SessionRepository sessionRepository, IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<SessionEntity> GetAll()
    {
        string sqlStatement = "SELECT * FROM session";
        IEnumerable<SessionEntity> sessions = null;

        try
        {
            sessions = _sessionRepository.GetAll(sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return sessions;
    }

    public SessionEntity GetByGuid(Guid guid)
    {
        string sqlStatement = "SELECT * FROM session WHERE Id = @Id";
        SessionEntity session = null;

        try
        {
            session = _sessionRepository.GetByGuid(guid, sqlStatement);
        }
        catch (Exception e)
        {
            throw e;
        }

        return session;
    }

    public int Insert(SessionEntity session)
    {
        string sqlStatement = "INSERT INTO session (Id, fk_player_id) VALUES (@Id, @FkPlayerId)";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _sessionRepository.Insert(session, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }

    public int Update(SessionEntity session)
    {
        string sqlStatement = "UPDATE session SET fk_player_id = @FkPlayerId WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _sessionRepository.Update(session, sqlStatement, transaction);
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
        string sqlStatement = "DELETE FROM session WHERE Id = @Id";
        int rowsAffected = 0;

        try
        {
            NpgsqlTransaction transaction = _unitOfWork.BeginTransaction();
            rowsAffected = _sessionRepository.DeleteByGuid(guid, sqlStatement, transaction);
            _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            throw e;
        }

        return rowsAffected;
    }
}