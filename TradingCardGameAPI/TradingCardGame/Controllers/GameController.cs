using System.Net;
using BusinessLogic.Services;
using BusinessLogic.Utils;
using BusinessObjects.Entities;
using BusinessObjects.Game;
using BusinessObjects.Interfaces.Controllers;

namespace BusinessLogic.Controllers;

public class GameController : IGameController
{
    public static GameController Instance
    {
        get
        {
            _instance ??= new GameController(CollectionService.Instance, GameService.Instance, PlayerService.Instance,
                SessionService.Instance);

            return _instance;
        }
    }

    private static GameController _instance = null;

    private CollectionService _collectionService;
    private GameService _gameService;
    private PlayerService _playerService;
    private SessionService _sessionService;

    private Queue<Player> _players = new Queue<Player>();

    public GameController(CollectionService collectionService, GameService gameService, PlayerService playerService, SessionService sessionService)
    {
        _collectionService = collectionService;
        _gameService = gameService;
        _playerService = playerService;
        _sessionService = sessionService;
    }

    // TODO Make the game asynchronous so that two players are matched up
    public void Play(HttpListenerContext ctx)
    {
        // Check if session is set
        _sessionService.CheckSession(ctx);

        // Get player id 
        var playerId = _sessionService.GetPlayerId(ctx);

        var PAUL_ID = new Guid("a89e4377-17d2-4081-9220-7a2374179f23");

        // Get player entity
        var playerEntity = _playerService.GetPlayerEntityByGuid(playerId);

        var PAUL_PLAYER_ENTITY = _playerService.GetPlayerEntityByGuid(PAUL_ID);

        // Get player deck
        var playerDeck = new List<CardEntity>(_collectionService.GetDeckByPlayerId(playerId));

        var PAUL_DECK = new List<CardEntity>(_collectionService.GetDeckByPlayerId(PAUL_ID));

        // Check whether the player deck has 4 cards
        if (playerDeck.Count != 4)
        {
            Response.Instance.BadRequest(ctx.Response);

            return;
        }

        // Create player entity
        var player = new Player(playerEntity, playerDeck);

        var PAUL_PLAYER = new Player(PAUL_PLAYER_ENTITY, PAUL_DECK);

        _players.Enqueue(player);
        _players.Enqueue(PAUL_PLAYER);

        if (_players.Count >= 2)
        {
            var player1 = _players.Dequeue();
            var player2 = _players.Dequeue();

            var log = _gameService.Start(player1, player2);

            // Send response message containing the log
            var res = ctx.Response;
            res.StatusCode = (int)HttpStatusCode.OK;
            res.ContentType = "text/plain";

            // Construct response body
            string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(log.Logs);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            // Set the content length
            res.ContentLength64 = buffer.Length;

            // Send response
            res.OutputStream.Write(buffer, 0, buffer.Length);
            res.OutputStream.Close();
        }
    }

}