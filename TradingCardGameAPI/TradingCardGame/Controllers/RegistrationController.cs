using System.Net;
using System.Security.Cryptography;
using BusinessLogic.Utils;
using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Controllers;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Facades;

namespace BusinessLogic.Controllers;

public class RegistrationController : IRegistrationController
{
    public static RegistrationController Instance
    {
        get
        {
            _instance ??= new RegistrationController(PlayerFacade.Instance);

            return _instance;
        }
    }

    private static RegistrationController _instance = null;

    private IPlayerFacade _playerFacade;

    public RegistrationController(IPlayerFacade playerFacade)
    {
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

        // Construct session cookie
        var cookie = new Cookie("session-id", Guid.NewGuid().ToString());
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
}