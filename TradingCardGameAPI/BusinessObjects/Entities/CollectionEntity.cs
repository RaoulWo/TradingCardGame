using BusinessObjects.Base;
using BusinessObjects.Interfaces;

namespace BusinessObjects.Entities;

public class CollectionEntity : Entity, IAggregateRoot
{
    public Guid FkPlayerId { get; set; }
    public Guid FkCardId { get; set; }

    protected override void Validate()
    {
        throw new NotImplementedException();
    }
}