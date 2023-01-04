using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Facades;
using BusinessObjects.Interfaces.Services;
using DataAccess.Facades;

namespace BusinessLogic.Services;

public class SessionService : ISessionService
{
    public static SessionService Instance
    {
        get
        {
            _instance ??= new SessionService(SessionFacade.Instance);

            return _instance;
        }
    }

    private static SessionService _instance = null;

    private ISessionFacade _sessionFacade;

    public SessionService(ISessionFacade sessionFacade)
    {
        _sessionFacade = sessionFacade;
    }

    public void CreateSession(SessionEntity session)
    {
        // Destroy previous session if exists
        DestroySessionByFkPlayerId(session.FkPlayerId);

        // Insert new session into db
        _sessionFacade.Insert(session);
    }

    public void DestroySessionById(Guid id)
    {
        _sessionFacade.DeleteByGuid(id);
    }

    public void DestroySessionByFkPlayerId(Guid fkPlayerId)
    {
        var sessions = _sessionFacade.GetAll();

        var session = sessions.FirstOrDefault(session => session.FkPlayerId == fkPlayerId);

        if (session?.Id != null)
        {
            _sessionFacade.DeleteByGuid((Guid)session.Id);
        }
    }

    public bool CheckSessionById(Guid id)
    {
        var session = _sessionFacade.GetByGuid(id);

        return session?.Id != null;
    }

    public bool CheckSessionByFkPlayerId(Guid fkPlayerId)
    {
        var sessions = _sessionFacade.GetAll();

        var session = sessions.FirstOrDefault(session => session.FkPlayerId == fkPlayerId);

        return session?.Id != null;
    }

    public Guid GetFkPlayerIdById(Guid id)
    {
        var session = _sessionFacade.GetByGuid(id);

        return (Guid)session.FkPlayerId;
    }
}