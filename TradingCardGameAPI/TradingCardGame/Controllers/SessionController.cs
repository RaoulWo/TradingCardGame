using System.Net;
using System.Security.Cryptography;
using BusinessLogic.Services;
using BusinessLogic.Utils;
using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Controllers;
using BusinessObjects.Interfaces.Facades;
using BusinessObjects.Interfaces.Services;
using DataAccess.Facades;

namespace BusinessLogic.Controllers;

public class SessionController : ISessionController
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

    /// <summary>
    /// Handles the player login and establishes a session if successful.
    /// </summary>
    /// <param name="ctx"></param>
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

        // Generate new session-id
        var sessionId = Guid.NewGuid();
        // Construct sessionEntity
        var sessionEntity = new SessionEntity
        {
            Id = sessionId,
            PlayerId = player.Id
        };
        // Store the sessionEntity in the db
        _sessionService.StoreSession(sessionEntity);

        // Construct session cookie
        var cookie = new Cookie("session-id", sessionId.ToString());
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

    /// <summary>
    /// Handles the player logout and ends the session if set.
    /// </summary>
    /// <param name="ctx"></param>
    public void Logout(HttpListenerContext ctx)
    {
        // Get request cookies
        var req = ctx.Request;
        var cookies = req.Cookies;

        // Check if session cookies are set if not send response with status code 400
        if (cookies.Count == 0)
        {
            // Send response with status code 400 Bad Request
            Response.Instance.BadRequest(ctx.Response);
            return;
        }
        
        // Extract session id from cookie
        var sessionId = new Guid(cookies[0].ToString().Substring(11));
        // Construct sessionEntity
        var sessionEntity = new SessionEntity()
        {
            Id = sessionId
        };
        // Destroy an existing session if set
        _sessionService.DestroySession(sessionEntity);
        
        // Construct response
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "text/plain";

        // Construct response message
        var responseString = "Logout successful!";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }

    /// <summary>
    /// Verifies the user entered password with the stored password
    /// </summary>
    /// <param name="userEnteredPwd"></param>
    /// <param name="storedPwd"></param>
    /// <returns></returns>
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