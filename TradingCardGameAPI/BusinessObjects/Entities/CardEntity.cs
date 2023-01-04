using BusinessObjects.Base;
using BusinessObjects.Enums;
using BusinessObjects.Interfaces;

namespace BusinessObjects.Entities;

public class CardEntity : Entity, IAggregateRoot
{
    public string Name { get; set; }
    public CardType Type { get; set; }
    public CardElement Element { get; set; }
    public int Damage { get; set; }

    protected override void Validate()
    {
        throw new NotImplementedException();
    }
}