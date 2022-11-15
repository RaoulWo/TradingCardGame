using BusinessObjects.Entities;
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

    private SessionFacade _sessionFacade;

    public SessionService(SessionFacade sessionFacade)
    {
        _sessionFacade = sessionFacade;
    }

    /// <summary>
    /// Destroys the session entity if set.
    /// </summary>
    /// <param name="sessionEntity"></param>
    public void DestroySession(SessionEntity sessionEntity)
    {
        // Get all session entities
        var sessions = _sessionFacade.GetAll();
        
        // Filter the session for the player id
        var session = sessions.FirstOrDefault(session => session.Id == sessionEntity.Id);
        
        // If session exists then delete the entry
        if (session != null)
        {
            _sessionFacade.DeleteByGuid(session.Id);
        }
    }

    /// <summary>
    /// Stores the new session entity and destroys previous if set.
    /// </summary>
    /// <param name="sessionEntity"></param>
    public void StoreSession(SessionEntity sessionEntity)
    {
        // Get all session entities
        var sessions = _sessionFacade.GetAll();
        
        // Filter the session for the player id
        var session = sessions.FirstOrDefault(session => session.PlayerId == sessionEntity.PlayerId);
        
        // If session exists then delete the entry
        if (session != null)
        {
            _sessionFacade.DeleteByGuid(session.Id);
        }
        
        // Insert the new session entity into the database
        _sessionFacade.Insert(sessionEntity);
    }
    
    
}