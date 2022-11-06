using BusinessObjects.Base;
using BusinessObjects.Interfaces;

namespace BusinessObjects.Entities;

public class PlayerEntity : Entity, IAggregateRoot
{
    public string Name { get; set; }
    public string Password { get; set; }
    public int Coins { get; set; }

    protected override void Validate()
    {
        throw new NotImplementedException();
    }
}