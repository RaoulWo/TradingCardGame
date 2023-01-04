using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Facades;
using BusinessObjects.Interfaces.Services;
using DataAccess.Facades;
using Microsoft.VisualBasic.CompilerServices;

namespace BusinessLogic.Services;

public class StoreService : IStoreService
{
    public static StoreService Instance
    {
        get
        {
            _instance ??= new StoreService(CardFacade.Instance);

            return _instance;
        }
    }

    private static StoreService _instance = null;

    private ICardFacade _cardFacade;

    public StoreService(ICardFacade cardFacade)
    {
        _cardFacade = cardFacade;
    }

    public List<CardEntity> GeneratePackage()
    {
        var cards = new List<CardEntity>(_cardFacade.GetAll());

        var package = new List<CardEntity>();

        const int packageSize = 5;

        Random r = new Random();
        
        for (int i = 0; i < packageSize; i++)
        {
            int randomIndex = r.Next(0, cards.Count);

            package.Add(cards[randomIndex]);
        }

        return package;
    }
}