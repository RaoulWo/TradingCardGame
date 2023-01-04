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

    public PlayerEntity GetPlayerEntityByGuid(Guid guid)
    {
        return _playerFacade.GetByGuid(guid);
    }

    public void ReducePlayerCoinsBy5(PlayerEntity player)
    {
        player.Coins -= 5;

        _playerFacade.Update(player);
    }
}