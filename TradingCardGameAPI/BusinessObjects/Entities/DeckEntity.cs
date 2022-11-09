using BusinessObjects.Base;
using BusinessObjects.Interfaces;

namespace BusinessObjects.Entities;

public class DeckEntity : Entity, IAggregateRoot
{
    public Guid PlayerId { get; set; }
    public Guid CollectionId { get; set; }

    protected override void Validate()
    {
        throw new NotImplementedException();
    }
}