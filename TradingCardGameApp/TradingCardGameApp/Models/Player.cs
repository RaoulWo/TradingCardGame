
namespace TradingCardGameApp.Models;

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Password { get; set; }
    public int Coins { get; set; }

    public Player(string name, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Password = password;
        Coins = 20;
    }

    public Player(Guid id, string name, string password, int coins)
    {
        Id = id;
        Name = name;
        Password = password;
        Coins = coins;
    }
}