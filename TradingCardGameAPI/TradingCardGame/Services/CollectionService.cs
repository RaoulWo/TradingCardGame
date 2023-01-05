using BusinessObjects.Entities;
using DataAccess.Facades;
using Microsoft.VisualBasic;

namespace BusinessLogic.Services;

public class CollectionService
{
    public static CollectionService Instance
    {
        get
        {
            _instance ??= new CollectionService(CardFacade.Instance, CollectionFacade.Instance, DeckFacade.Instance);

            return _instance;
        }
    }

    private static CollectionService _instance = null;

    private CardFacade _cardFacade;
    private CollectionFacade _collectionFacade;
    private DeckFacade _deckFacade;

    public CollectionService(CardFacade cardFacade, CollectionFacade collectionFacade, DeckFacade deckFacade)
    {
        _cardFacade = cardFacade;
        _collectionFacade = collectionFacade;
        _deckFacade = deckFacade;
    }

    public IEnumerable<CardEntity> GetCollectionByPlayerId(Guid playerId)
    {
        // Get all collection entities of player
        var allCollections = _collectionFacade.GetAll();
        var collections = allCollections.Where(collection => collection.FkPlayerId == playerId);

        // Get all cards with fk card id
        var allCards = _cardFacade.GetAll();
        var cards = allCards.Join(collections,
            card => card.Id,
            collection => collection.FkCardId,
            (card, _) => new CardEntity()
            {
                Id = card.Id,
                Damage = card.Damage,
                Element = card.Element,
                Name = card.Name,
                Type = card.Type,
            });

        return cards;
    }

    public IEnumerable<CollectionEntity> GetCollectionEntitiesByPlayerId(Guid playerId)
    {
        // Get all collection entities of player
        var allCollections = _collectionFacade.GetAll();
        var collections = allCollections.Where(collection => collection.FkPlayerId == playerId);

        return collections;
    }

    public IEnumerable<CardEntity> GetDeckByPlayerId(Guid playerId)
    {
        // Get all deck entities
        var allDecks = _deckFacade.GetAll();

        // Get all collection entities of player
        var allCollections = _collectionFacade.GetAll();
        var collections = allCollections.Where(collection => collection.FkPlayerId == playerId);

        // Inner join the collections with the deck entities
        var deckCollections = allDecks.Join(collections,
            deck => deck.FkCollectionId,
            collection => collection.Id,
            (deck, collection) => new CollectionEntity()
            {
                Id = collection.Id,
                FkCardId = collection.FkCardId,
                FkPlayerId = collection.FkPlayerId,
            });

        // Get all cards
        var allCards = _cardFacade.GetAll();
        var cards = allCards.Join(deckCollections,
            card => card.Id,
            collection => collection.FkCardId,
            (card, _) => new CardEntity()
            {
                Id = card.Id,
                Damage = card.Damage,
                Element = card.Element,
                Name = card.Name,
                Type = card.Type,
            });

        return cards;
    }

    public void StorePackageInCollection(Guid fkPlayerId, List<CardEntity> package)
    {
        foreach (var card in package)
        {
            // Create collection entity
            var collectionEntity = new CollectionEntity()
            {
                FkPlayerId = fkPlayerId,
                FkCardId = (Guid)card.Id
            };

            // Store the collection entity in the db
            _collectionFacade.Insert(collectionEntity);
        }
    }

    public bool CheckIfCardIsInCollection(CollectionEntity card, List<CollectionEntity> collection)
    {
        foreach (var c in collection)
        {
            if (card == c)
            {
                return true;
            }
        }

        return false;
    }

    public void DeleteDeckByPlayerId(Guid fkPlayerId)
    {
        // Get all deck entities
        var decks = _deckFacade.GetAll();

        // Get all collections of player
        var collections = _collectionFacade.GetAll();
        var playerCollections = collections.Where(collection => collection.FkPlayerId == fkPlayerId);

        // Filter all playerDecks 
        var playerDecks = decks.Join(playerCollections,
            deck => deck.FkCollectionId,
            collection => collection.Id,
            (deck, collection) => new DeckEntity()
            {
                Id = deck.Id,
                FkCollectionId = deck.FkCollectionId,
            });

        // Delete all decks of player
        foreach (var deck in playerDecks)
        {
            _deckFacade.DeleteByGuid((Guid)deck.Id);
        }
    }

    public void InsertDecks(List<CollectionEntity> collections)
    {
        foreach (var collection in collections)
        {
            var deckToInsert = new DeckEntity()
            {
                FkCollectionId = (Guid)collection.Id
            };

            _deckFacade.Insert(deckToInsert);
        }
    }
}