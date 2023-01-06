using System.Security.Cryptography;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using BusinessObjects.Game;
using BusinessObjects.Interfaces.Services;
using DataAccess.Facades;

namespace BusinessLogic.Services;

public class GameService : IGameService
{
    public static GameService Instance
    {
        get
        {
            _instance ??= new GameService(PlayerFacade.Instance);

            return _instance;
        }
    }

    private static GameService _instance = null;
    
    private PlayerFacade _playerFacade;

    public GameService(PlayerFacade playerFacade)
    {
        _playerFacade = playerFacade;
    }

    public Log Start(Player player1, Player player2)
    {
        var log = new Log();

        const int ROUND_LIMIT = 100;

        for (int i = 1; i <= ROUND_LIMIT; i++)
        {
            // Check if players have cards left
            if (player1.DeckIsEmpty() || player2.DeckIsEmpty())
            {
                log.AddEntry("Battle is over!\n");

                if (player1.WonRounds > player2.WonRounds)
                {
                    log.AddEntry("Player A Wins!\n");

                    UpdateElo(player1.PlayerObject, player2.PlayerObject);
                    UpdateWins(player1.PlayerObject);
                    UpdateLosses(player2.PlayerObject);
                }
                else if (player1.WonRounds < player2.WonRounds)
                {
                    log.AddEntry("Player B Wins!\n");

                    UpdateElo(player2.PlayerObject, player1.PlayerObject);
                    UpdateWins(player2.PlayerObject);
                    UpdateLosses(player1.PlayerObject);
                }
                else
                {
                    log.AddEntry("Draw!\n");

                    UpdateDraws(player1.PlayerObject);
                    UpdateDraws(player2.PlayerObject);
                }

                break;
            }

            // Draw card of each player
            var cardFromPlayer1 = player1.DrawCard();
            var cardFromPlayer2 = player2.DrawCard();

            // Compare the cards 
            int result = CompareCards(cardFromPlayer1, cardFromPlayer2, log);

            if (result == 1)
            {
                // Increase rounds won
                player1.WonRounds++;
                // Add card from other player
                player1.AddCard(cardFromPlayer2);
            }
            else if (result == -1)
            {
                // Increase rounds won
                player2.WonRounds++;
                // Add card from other player
                player2.AddCard(cardFromPlayer1);
            }
        }

        return log;
    }

    // Returns 1 if player1 won round, -1 if player2 won round, 0 if draw
    public int CompareCards(CardEntity card1, CardEntity card2, Log log)
    {
        var cardOne = card1;
        var cardTwo = card2;

        // Monster fight
        if (cardOne.Type == CardType.Monster && cardTwo.Type == CardType.Monster)
        {
            // Exceptions
            if (cardOne.Name == "Dragon" && cardTwo.Name == "Goblin")
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                return 1;
            }
            else if (cardOne.Name == "Wizard" && cardTwo.Name == "Ork")
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                return 1;
            }
            else if (cardOne.Name == "Elf" && cardOne.Element == CardElement.Fire && cardTwo.Name == "Dragon")
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                return 1;
            }
            else if (cardTwo.Name == "Dragon" && cardOne.Name == "Goblin")
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                return -1;
            }
            else if (cardTwo.Name == "Wizard" && cardOne.Name == "Ork")
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                return -1;
            }
            else if (cardTwo.Name == "Elf" && cardTwo.Element == CardElement.Fire && cardOne.Name == "Dragon")
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                return -1;
            }
            // Normal calculation

            if (cardOne.Damage > cardTwo.Damage)
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                return 1;
            }
            else if (cardTwo.Damage > cardOne.Damage)
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                return -1;
            }
            else
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} Draw");

                return 0;
            }
        }
        // Spell fight
        else if (cardOne.Type == CardType.Spell && cardTwo.Type == CardType.Spell)
        {
            // Normal calculation
            if (cardOne.Element == cardTwo.Element)
            {
                if (cardOne.Damage > cardTwo.Damage)
                {
                    log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                    return 1;
                }
                else if (cardTwo.Damage > cardOne.Damage)
                {
                    log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                    return -1;
                }
                else
                {
                    log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} Draw");

                    return 0;
                }
            }

            // Mixed elements
            if (cardOne.Element == CardElement.Fire && cardTwo.Element == CardElement.Normal ||
                cardOne.Element == CardElement.Normal && cardTwo.Element == CardElement.Water ||
                cardOne.Element == CardElement.Water && cardTwo.Element == CardElement.Fire)
            {
                cardOne.Damage *= 2;
                cardTwo.Damage /= 2;
            }
            else if (cardTwo.Element == CardElement.Fire && cardOne.Element == CardElement.Normal ||
                     cardTwo.Element == CardElement.Normal && cardOne.Element == CardElement.Water ||
                     cardTwo.Element == CardElement.Water && cardOne.Element == CardElement.Fire)
            {
                cardTwo.Damage *= 2;
                cardOne.Damage /= 2;
            }

            if (cardOne.Damage > cardTwo.Damage)
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                return 1;
            }
            else if (cardTwo.Damage > cardOne.Damage)
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                return -1;
            }
            else
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} Draw");

                return 0;
            }
        }
        // Mixed fight
        else
        {
            // Exceptions
            if (cardOne.Name == "Spell" && cardOne.Element == CardElement.Water && cardTwo.Name == "Knight")
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                return 1;
            }
            else if (cardOne.Name == "Kraken" && cardTwo.Type == CardType.Spell)
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                return 1;
            }
            else if (cardTwo.Name == "Spell" && cardTwo.Element == CardElement.Water && cardOne.Name == "Knight")
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                return -1;
            }
            else if (cardTwo.Name == "Kraken" && cardOne.Type == CardType.Spell)
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                return -1;
            }

            // Normal calculation
            if (cardOne.Element == cardTwo.Element)
            {
                if (cardOne.Damage > cardTwo.Damage)
                {
                    log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                    return 1;
                }
                else if (cardTwo.Damage > cardOne.Damage)
                {
                    log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                    return -1;
                }
                else
                {
                    log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} Draw");

                    return 0;
                }
            }

            // Mixed elements
            if (cardOne.Element == CardElement.Fire && cardTwo.Element == CardElement.Normal ||
                cardOne.Element == CardElement.Normal && cardTwo.Element == CardElement.Water ||
                cardOne.Element == CardElement.Water && cardTwo.Element == CardElement.Fire)
            {
                cardOne.Damage *= 2;
                cardTwo.Damage /= 2;
            }
            else if (cardTwo.Element == CardElement.Fire && cardOne.Element == CardElement.Normal ||
                     cardTwo.Element == CardElement.Normal && cardOne.Element == CardElement.Water ||
                     cardTwo.Element == CardElement.Water && cardOne.Element == CardElement.Fire)
            {
                cardTwo.Damage *= 2;
                cardOne.Damage /= 2;
            }

            if (cardOne.Damage > cardTwo.Damage)
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardOne.Name} defeats {cardTwo.Name}");

                return 1;
            }
            else if (cardTwo.Damage > cardOne.Damage)
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} {cardTwo.Name} defeats {cardOne.Name}");

                return -1;
            }
            else
            {
                log.AddEntry($"{GetFirstPartOfLogEntry(cardOne, cardTwo)} Draw");

                return 0;
            }

        }
    }

    public string GetFirstPartOfLogEntry(CardEntity card1, CardEntity card2)
    {
        return $"Player A: {card1.Element.ToString()} {card1.Name} ({card1.Damage} Damage) vs Player B: {card2.Element.ToString()} {card2.Name} ({card2.Damage} Damage) => ";
    }

    public void UpdateElo(PlayerEntity winner, PlayerEntity loser)
    {
        winner.Elo += 3;
        loser.Elo -= 5;

        _playerFacade.Update(winner);
        _playerFacade.Update(loser);
    }

    public void UpdateWins(PlayerEntity winner)
    {
        winner.Wins += 1;

        _playerFacade.Update(winner);
    }

    public void UpdateLosses(PlayerEntity loser)
    {
        loser.Losses += 1;

        _playerFacade.Update(loser);
    }

    public void UpdateDraws(PlayerEntity player)
    {
        player.Draws += 1;

        _playerFacade.Update(player);
    }
}