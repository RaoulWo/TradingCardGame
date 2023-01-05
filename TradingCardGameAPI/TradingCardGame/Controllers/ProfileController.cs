using System.Net;
using BusinessLogic.Services;
using BusinessLogic.Utils;
using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Controllers;

namespace BusinessLogic.Controllers;

public class ProfileController : IProfileController
{
    public static ProfileController Instance
    {
        get
        {
            _instance ??= new ProfileController(PlayerService.Instance, SessionService.Instance);

            return _instance;
        }
    }

    private static ProfileController _instance = null;

    private PlayerService _playerService;
    private SessionService _sessionService;

    public ProfileController(PlayerService playerService, SessionService sessionService)
    {
        _playerService = playerService;
        _sessionService = sessionService;
    }

    public void ShowProfile(HttpListenerContext ctx)
    {
        // Check if session is set
        _sessionService.CheckSession(ctx);

        // Get the player id
        var playerId = _sessionService.GetPlayerId(ctx);

        // Get the player entity
        var player = _playerService.GetPlayerEntityByGuid(playerId);
        // Remove the password
        player.Password = null;

        // Send response message containing the player entity
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct response body
        string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(player);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }

    public void ConfigureProfile(HttpListenerContext ctx)
    {
        // Check if session is set
        _sessionService.CheckSession(ctx);

        // Get request body
        var req = ctx.Request;
        var body = new StreamReader(req.InputStream).ReadToEnd();

        // Convert request body to player entity
        var playerToUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerEntity>(body);

        // Get the player id
        var playerId = _sessionService.GetPlayerId(ctx);

        // Check if the new player name is already taken
        var nameIsAvailable = _playerService.CheckIfUsernameIsAvailable(playerToUpdate.Name);
        // If already taken send Conflict response
        if (!nameIsAvailable)
        {
            Response.Instance.Conflict(ctx.Response);

            return;
        }

        // Hash the updated password
        playerToUpdate.Password = _sessionService.Hash(playerToUpdate.Password);

        // Update the user
        _playerService.UpdatePlayerEntity(playerToUpdate);

        // Query the updated user
        var updatedPlayer = _playerService.GetPlayerEntityByGuid(playerId);
        updatedPlayer.Password = null;

        // Send response message containing the collection
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct response body
        string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(updatedPlayer);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }
}