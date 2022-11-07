using System.Net;
using System.Security.Cryptography;
using BusinessLogic.Utils;
using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Controllers;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Facades;

namespace BusinessLogic.Controllers;

public class LoginController : ILoginController
{
    public static LoginController Instance
    {
        get
        {
            _instance ??= new LoginController(PlayerFacade.Instance);

            return _instance;
        }
    }

    private static LoginController _instance = null;

    private IPlayerFacade _playerFacade;

    public LoginController(IPlayerFacade playerFacade)
    {
        _playerFacade = playerFacade;
    }

    public void Post(HttpListenerContext ctx)
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
        res.ContentType = "text/plain";

        // Construct session cookie
        var cookie = new Cookie("session-id", Guid.NewGuid().ToString());
        res.AppendCookie(cookie);

        // Construct response message
        string responseString = "Login successful!";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
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