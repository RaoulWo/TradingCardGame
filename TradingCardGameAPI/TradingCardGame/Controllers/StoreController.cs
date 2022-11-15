using System.Net;
using BusinessObjects.Interfaces.Controllers;
using BusinessObjects.Interfaces.Facades;
using DataAccess.Facades;

namespace BusinessLogic.Controllers;

public class StoreController : IStoreController
{
    public static StoreController Instance
    {
        get
        {
            _instance ??= new StoreController(PlayerFacade.Instance, CardFacade.Instance);

            return _instance;
        }
    }
    
    private static StoreController _instance = null;

    private IPlayerFacade _playerFacade;
    private ICardFacade _cardFacade;

    public StoreController(IPlayerFacade playerFacade, ICardFacade cardFacade)
    {
        _playerFacade = playerFacade;
        _cardFacade = cardFacade;
    }

    public void Buy(HttpListenerContext ctx)
    {
        // TODO Get Request Authorization Header ...
    }
}