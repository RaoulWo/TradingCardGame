using BusinessLogic.Utils;
using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Facades;
using BusinessObjects.Interfaces.Services;
using DataAccess.Facades;
using System.Net;
using System.Security.Cryptography;

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

    public void CheckSession(HttpListenerContext ctx)
    {
        // Get the session cookie
        var req = ctx.Request;
        var sessionCookie = req.Cookies[0];

        // Store the session id 
        var sessionId = new Guid(sessionCookie.Value);

        // Check if session exists
        var sessionExists = CheckSessionById(sessionId);

        // Send unauthorized response if session does not exist
        if (!sessionExists)
        {
            Response.Instance.Unauthorized(ctx.Response);
        }
    }

    public Guid GetPlayerId(HttpListenerContext ctx)
    {
        return GetFkPlayerIdById(GetSessionId(ctx));
    }

    public Guid GetSessionId(HttpListenerContext ctx)
    {
        // Get the session cookie
        var req = ctx.Request;
        var sessionCookie = req.Cookies[0];

        // Store the session id 
        var sessionId = new Guid(sessionCookie.Value);

        return sessionId;
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


    public string Hash(string password)
    {
        // Create a salt value
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        // Create the Rfc2898DeriveBytes and get the hash value
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);

        // Combine the salt and password bytes
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        // Convert the combined salt and hash into a string
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string userEnteredPwd, string storedPwd)
    {
        // Extract the bytes of the stored password
        byte[] hashBytes = Convert.FromBase64String(storedPwd);

        // Extract the salt 
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Compute the hash on the user entered password
        var pbkdf2 = new Rfc2898DeriveBytes(userEnteredPwd, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);

        // Compare the results
        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}