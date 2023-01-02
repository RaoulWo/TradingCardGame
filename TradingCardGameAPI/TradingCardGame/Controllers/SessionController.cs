using BusinessLogic.Utils;
using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Facades;
using System.Net;
using System.Security.Cryptography;
using BusinessLogic.Services;
using BusinessObjects.Interfaces.Services;
using System;

namespace BusinessLogic.Controllers;

public class SessionController
{
    public static SessionController Instance
    {
        get
        {
            _instance ??= new SessionController(SessionService.Instance, PlayerFacade.Instance);

            return _instance;
        }
    }

    private static SessionController _instance = null;

    private ISessionService _sessionService;
    private IPlayerFacade _playerFacade;

    public SessionController(ISessionService sessionService, IPlayerFacade playerFacade)
    {
        _sessionService = sessionService;
        _playerFacade = playerFacade;
    }

    public void Register(HttpListenerContext ctx)
    {
        // Get request body
        var req = ctx.Request;
        var body = new StreamReader(req.InputStream).ReadToEnd();

        // Convert request body to player entity
        var insertPlayer = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerEntity>(body);

        // Get all players
        var players = _playerFacade.GetAll();

        // Check if player with name of insertPlayer already exists
        var player = players.FirstOrDefault(p => p.Name == insertPlayer?.Name);
        if (player != null)
        {
            // Send response with status code 409 Conflict
            Response.Instance.Conflict(ctx.Response);
            return;
        }

        // Hash the password
        string hashedPassword = Hash(insertPlayer.Password);
        insertPlayer.Password = hashedPassword;

        // Try to insert the user in the db
        int rowsAffected = 0;
        try
        {
            rowsAffected = _playerFacade.Insert(insertPlayer);
        }
        catch (Exception e)
        {
            throw e;
        }

        if (rowsAffected == 0)
        {
            // Send response with status code 500 Internal Server Error
            Response.Instance.InternalServerError(ctx.Response);
            return;
        }

        // Update players
        players = _playerFacade.GetAll();

        // Find player that has been inserted
        player = players.FirstOrDefault(p => p.Name == insertPlayer.Name);
        player.Password = null;

        // Construct response
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct session object
        var session = new SessionEntity
        {
            Id = Guid.NewGuid(),
            FkPlayerId = (Guid)player.Id
        };

        // Create session
        _sessionService.CreateSession(session);

        // Construct session cookie
        var cookie = new Cookie("session-id", session.Id.ToString());
        res.AppendCookie(cookie);

        // Construct response message
        string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(player);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }

    public void Login(HttpListenerContext ctx)
    {
        // Get request body
        var req = ctx.Request;
        var body = new StreamReader(req.InputStream).ReadToEnd();

        // Convert request body to player entity
        var loginPlayer = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerEntity>(body);

        // Get all players
        var players = _playerFacade.GetAll();

        // Check if player with name of player exists
        var player = players.FirstOrDefault(p => p.Name == loginPlayer?.Name);
        if (player == null)
        {
            // Send response with status code 401 Unauthorized
            Response.Instance.Unauthorized(ctx.Response);
            return;
        }

        var passwordIsMatching = VerifyPassword(loginPlayer.Password, player.Password);
        if (!passwordIsMatching)
        {
            // Send response with status code 401 Unauthorized
            Response.Instance.Unauthorized(ctx.Response);
            return;
        }

        // Construct response
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct session object
        var session = new SessionEntity
        {
            Id = Guid.NewGuid(),
            FkPlayerId = (Guid)player.Id
        };

        // Create session
        _sessionService.CreateSession(session);

        // Construct session cookie
        var cookie = new Cookie("session-id", session.Id.ToString());
        res.AppendCookie(cookie);

        // Construct response message
        player.Password = null;
        var responseString = Newtonsoft.Json.JsonConvert.SerializeObject(player);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }

    public void Logout(HttpListenerContext ctx)
    {
        // Get the session cookie
        var req = ctx.Request;
        var sessionCookie = req.Cookies[0];

        // Destroy the session
        if (sessionCookie != null)
        {
            _sessionService.DestroySessionById(new Guid(sessionCookie.Value));
        }

        // Construct response
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Send response
        res.OutputStream.Write(new byte[] {}, 0, 0);
        res.OutputStream.Close();
    }

    private string Hash(string password)
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

    private bool VerifyPassword(string userEnteredPwd, string storedPwd)
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