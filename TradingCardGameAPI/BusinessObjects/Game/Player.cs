using BusinessObjects.Entities;

namespace BusinessObjects.Game;

public class Player
{
    public PlayerEntity PlayerObject { get; }

    public int WonRounds { get; set; } = 0;

    private List<CardEntity> _deck;

    public Player(PlayerEntity player, List<CardEntity> deck)
    {
        PlayerObject = player;
        _deck = deck;
    }

    public CardEntity DrawCard()
    {
        if (DeckIsEmpty())
        {
            return null;
        }

        var rnd = new Random();

        var randomIndex = rnd.Next(0, _deck.Count);

        var card = _deck[randomIndex];

        _deck.RemoveAt(randomIndex);

        return card;
    }

    public void AddCard(CardEntity card)
    {
        _deck.Add(card);
    }

    public bool DeckIsEmpty()
    {
        return _deck.Count == 0;
    }
}