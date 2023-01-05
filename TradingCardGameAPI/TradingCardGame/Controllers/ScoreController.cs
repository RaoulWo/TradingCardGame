using System.Net;
using BusinessLogic.Services;
using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Controllers;

namespace BusinessLogic.Controllers;

public class ScoreController : IScoreController
{
    public static ScoreController Instance
    {
        get
        {
            _instance ??= new ScoreController(PlayerService.Instance, SessionService.Instance);

            return _instance;
        }
    }

    private static ScoreController _instance = null;

    private PlayerService _playerService;
    private SessionService _sessionService;

    public ScoreController(PlayerService playerService, SessionService sessionService)
    {
        _playerService = playerService;
        _sessionService = sessionService;
    }

    public void ShowLeaderBoard(HttpListenerContext ctx)
    {
        // Check if session is set
        _sessionService.CheckSession(ctx);

        // Get the players and order them by elo descending
        var players = _playerService.GetPlayerEntities();

        players = players.OrderByDescending(player => player.Elo);

        List<PlayerEntity> topPlayers = new List<PlayerEntity>();

        const int LEADERBOARD_SIZE = 10;

        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            topPlayers.Add(players.ToList()[i]);
        }

        // Send response message containing the top 10 players
        var res = ctx.Response;
        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "application/json";

        // Construct response body
        string responseString = Newtonsoft.Json.JsonConvert.SerializeObject(topPlayers);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Set the content length
        res.ContentLength64 = buffer.Length;

        // Send response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }
}