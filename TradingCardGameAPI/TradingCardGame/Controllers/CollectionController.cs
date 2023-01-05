using System.Net;
using BusinessLogic.Services;
using BusinessLogic.Utils;
using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Controllers;

namespace BusinessLogic.Controllers;

public class CollectionController : ICollectionController
{
    public static CollectionController Instance
    {
        get
        {
            _instance ??= new CollectionController(CollectionService.Instance, SessionService.Instance);

            return _instance;
        }
    }

    private static CollectionController _instance = null;

    private CollectionService _collectionService;
    private SessionService _sessionService;

    public CollectionController(CollectionService collectionService, SessionService sessionService)
    {
        _collectionService = collectionService;
        _sessionService = sessionService;
    }

    public void ShowCollection(HttpListenerContext ctx)
    {
        // Check if session is set
        _sessionService.CheckSession(ctx);

        var collection = _collectionService.GetCollectionByPlayerId(_sessionService.GetPlayerId(ctx));

        // Send response message containing the collection
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct response body
        string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(collection);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }

    public void ShowDeck(HttpListenerContext ctx)
    {
        // Check if session is set
        _sessionService.CheckSession(ctx);

        var deck = _collectionService.GetDeckByPlayerId(_sessionService.GetPlayerId(ctx));

        // Send response message containing the deck
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct response body
        string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(deck);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }

    public void ConfigureDeck(HttpListenerContext ctx)
    {
        // Check if session is set
        _sessionService.CheckSession(ctx);

        // Get request body
        var req = ctx.Request;
        var body = new StreamReader(req.InputStream).ReadToEnd();

        // Get player id
        var playerId = _sessionService.GetPlayerId(ctx);

        // Convert request body to list of card entities
        var newDeck = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CollectionEntity>>(body);

        // Check if the cards are actually in the collection
        foreach (var newDeckCard in newDeck)
        {
            bool deckIsValid = _collectionService.CheckIfCardIsInCollection(newDeckCard, 
                new List<CollectionEntity>(_collectionService.GetCollectionEntitiesByPlayerId(playerId)));
            if (!deckIsValid)
            {
                Response.Instance.Unauthorized(ctx.Response);

                return;
            }
        }

        // Delete the old deck by player id
        _collectionService.DeleteDeckByPlayerId(playerId);

        // Insert the new deck by player id
        _collectionService.InsertDecks(newDeck);

        var insertedDeck = _collectionService.GetDeckByPlayerId(playerId);

        // Send response message containing the newly inserted deck
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct response body
        string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(insertedDeck);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }
}