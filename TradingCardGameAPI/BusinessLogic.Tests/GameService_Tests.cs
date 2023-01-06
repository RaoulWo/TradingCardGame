using BusinessLogic.Services;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using BusinessObjects.Game;

namespace BusinessLogic.Tests;

public class GameService_Tests
{
    private Log log = new Log();

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Dragon_should_win_against_Goblin()
    {
        // Arrange
        CardEntity dragon = new CardEntity()
        {
            Name = "Dragon",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Monster
        };
        CardEntity goblin = new CardEntity()
        {
            Name = "Goblin",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(dragon, goblin, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Goblin_should_lose_against_Dragon()
    {
        // Arrange
        CardEntity dragon = new CardEntity()
        {
            Name = "Dragon",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Monster
        };
        CardEntity goblin = new CardEntity()
        {
            Name = "Goblin",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(goblin, dragon, log);

        // Assert
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void Wizard_should_win_against_Ork()
    {
        // Arrange
        CardEntity wizard = new CardEntity()
        {
            Name = "Wizard",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Monster
        };
        CardEntity ork = new CardEntity()
        {
            Name = "Ork",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(wizard, ork, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Ork_should_lose_against_Wizard()
    {
        // Arrange
        CardEntity wizard = new CardEntity()
        {
            Name = "Wizard",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Monster
        };
        CardEntity ork = new CardEntity()
        {
            Name = "Ork",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(ork, wizard, log);

        // Assert
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void Fire_Elf_should_win_against_Dragon()
    {
        // Arrange
        CardEntity fireElf = new CardEntity()
        {
            Name = "Elf",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };
        CardEntity dragon = new CardEntity()
        {
            Name = "Dragon",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(fireElf, dragon, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Dragon_should_lose_against_Fire_Elf()
    {
        // Arrange
        CardEntity fireElf = new CardEntity()
        {
            Name = "Elf",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };
        CardEntity dragon = new CardEntity()
        {
            Name = "Dragon",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(dragon, fireElf, log);

        // Assert
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void Higher_damage_monster_should_win()
    {
        // Arrange
        CardEntity dragon1 = new CardEntity()
        {
            Name = "Dragon",
            Damage = 15,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };
        CardEntity dragon2 = new CardEntity()
        {
            Name = "Dragon",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(dragon1, dragon2, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Lower_damage_monster_should_lose()
    {
        // Arrange
        CardEntity dragon1 = new CardEntity()
        {
            Name = "Dragon",
            Damage = 5,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };
        CardEntity dragon2 = new CardEntity()
        {
            Name = "Dragon",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(dragon1, dragon2, log);

        // Assert
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void Equal_damage_monster_should_draw()
    {
        // Arrange
        CardEntity dragon1 = new CardEntity()
        {
            Name = "Dragon",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };
        CardEntity dragon2 = new CardEntity()
        {
            Name = "Dragon",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(dragon1, dragon2, log);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Same_element_spell_should_behave_normally()
    {
        // Arrange
        CardEntity spell1 = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Spell
        };
        CardEntity spell2 = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Spell
        };


        // Act
        int result = GameService.Instance.CompareCards(spell1, spell2, log);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Fire_should_beat_Normal_Spell()
    {
        // Arrange
        CardEntity spell1 = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Spell
        };
        CardEntity spell2 = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Normal,
            Type = CardType.Spell
        };


        // Act
        int result = GameService.Instance.CompareCards(spell1, spell2, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Normal_should_beat_Water_Spell()
    {
        // Arrange
        CardEntity spell1 = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Normal,
            Type = CardType.Spell
        };
        CardEntity spell2 = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Spell
        };


        // Act
        int result = GameService.Instance.CompareCards(spell1, spell2, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Water_should_beat_Fire_Spell()
    {
        // Arrange
        CardEntity spell1 = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Spell
        };
        CardEntity spell2 = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Spell
        };


        // Act
        int result = GameService.Instance.CompareCards(spell1, spell2, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Water_Spell_should_win_against_Knight()
    {
        // Arrange
        CardEntity spell = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Spell
        };
        CardEntity knight = new CardEntity()
        {
            Name = "Knight",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(spell, knight, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Kraken_should_win_against_Spells()
    {
        // Arrange
        CardEntity spell = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Spell
        };
        CardEntity kraken = new CardEntity()
        {
            Name = "Kraken",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(kraken, spell, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Mixed_fight_with_same_elements_should_behave_normally()
    {
        // Arrange
        CardEntity spell = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Spell
        };
        CardEntity elf = new CardEntity()
        {
            Name = "Elf",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(elf, spell, log);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Fire_Spell_should_beat_normal_monster()
    {
        // Arrange
        CardEntity spell = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Spell
        };
        CardEntity elf = new CardEntity()
        {
            Name = "Elf",
            Damage = 10,
            Element = CardElement.Normal,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(spell, elf, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Water_Spell_should_beat_fire_monster()
    {
        // Arrange
        CardEntity spell = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Spell
        };
        CardEntity elf = new CardEntity()
        {
            Name = "Elf",
            Damage = 10,
            Element = CardElement.Fire,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(spell, elf, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Normal_Spell_should_beat_water_monster()
    {
        // Arrange
        CardEntity spell = new CardEntity()
        {
            Name = "Spell",
            Damage = 10,
            Element = CardElement.Normal,
            Type = CardType.Spell
        };
        CardEntity elf = new CardEntity()
        {
            Name = "Elf",
            Damage = 10,
            Element = CardElement.Water,
            Type = CardType.Monster
        };


        // Act
        int result = GameService.Instance.CompareCards(spell, elf, log);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
}