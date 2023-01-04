using System.Net;
using BusinessLogic.Services;
using BusinessLogic.Utils;
using BusinessObjects.Interfaces.Controllers;

namespace BusinessLogic.Controllers;

public class StoreController : IStoreController
{
    public static StoreController Instance 
    {
        get
        {
            _instance ??= new StoreController(CollectionService.Instance, PlayerService.Instance, SessionService.Instance, StoreService.Instance);

            return _instance;
        }
    }

    private static StoreController _instance = null;

    private CollectionService _collectionService;
    private PlayerService _playerService;
    private SessionService _sessionService;
    private StoreService _storeService;

    public StoreController(CollectionService collectionService, PlayerService playerService, SessionService sessionService, StoreService storeService)
    {
        _collectionService = collectionService;
        _playerService = playerService;
        _sessionService = sessionService;
        _storeService = storeService;
    }

    public void BuyPack(HttpListenerContext ctx)
    {
        // Get the session cookie
        var req = ctx.Request;
        var sessionCookie = req.Cookies[0];

        // Store the session id 
        var sessionId = new Guid(sessionCookie.Value);

        // Check if session exists
        var sessionExists = _sessionService.CheckSessionById(sessionId);

        // Send unauthorized response if session does not exist
        if (!sessionExists)
        {
            Response.Instance.Unauthorized(ctx.Response);

            return;
        }

        // Get the player id by session id
        var playerId = _sessionService.GetFkPlayerIdById(sessionId);

        // Get the player entity by id
        var player = _playerService.GetPlayerEntityByGuid(playerId);

        // Check if player has enough coins to buy a pack, if not then send bad request response
        if (player.Coins < 5)
        {
            Response.Instance.BadRequest(ctx.Response);

            return;
        }

        // Generate a pack of five cards
        var package = _storeService.GeneratePackage();

        // Add the acquired cards to the player collection
        _collectionService.StorePackageInCollection(playerId, package);

        // Reduce the coins of the player by 5
        _playerService.ReducePlayerCoinsBy5(player);

        // Send response message containing the five acquired cards
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct response body
        string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(package);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }
}