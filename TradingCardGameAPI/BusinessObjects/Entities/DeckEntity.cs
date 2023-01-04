using BusinessObjects.Base;
using BusinessObjects.Interfaces;

namespace BusinessObjects.Entities;

public class DeckEntity : Entity, IAggregateRoot
{
    public Guid FkCollectionId { get; set; }

    protected override void Validate()
    {
        throw new NotImplementedException();
    }
}