using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Services;
using DataAccess.Facades;

namespace BusinessLogic.Services;

public class PlayerService : IPlayerService
{
    public static PlayerService Instance
    {
        get
        {
            _instance ??= new PlayerService(PlayerFacade.Instance);

            return _instance;
        }
    }

    private static PlayerService _instance = null;

    private PlayerFacade _playerFacade;

    public PlayerService(PlayerFacade playerFacade)
    {
        _playerFacade = playerFacade;
    }

    public IEnumerable<PlayerEntity> GetPlayerEntities()
    {
        return _playerFacade.GetAll();
    }

    public PlayerEntity GetPlayerEntityByGuid(Guid guid)
    {
        return _playerFacade.GetByGuid(guid);
    }

    public bool CheckIfUsernameIsAvailable(string username)
    {
        var players = GetPlayerEntities();

        foreach (var player in players)
        {
            if (player.Name == username)
            {
                return false;
            }
        }

        return true;
    }

    public void UpdatePlayerEntity(PlayerEntity player)
    {
        _playerFacade.Update(player);
    }

    public void ReducePlayerCoinsBy5(PlayerEntity player)
    {
        player.Coins -= 5;

        if (player.Coins < 0)
        {
            player.Coins = 0;
        }

        _playerFacade.Update(player);
    }
}