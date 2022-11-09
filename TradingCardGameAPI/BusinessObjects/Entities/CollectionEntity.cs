using BusinessObjects.Base;
using BusinessObjects.Interfaces;

namespace BusinessObjects.Entities;

public class CollectionEntity : Entity, IAggregateRoot
{
    public Guid PlayerId { get; set; }
    public Guid CardId { get; set; }

    protected override void Validate()
    {
        throw new NotImplementedException();
    }
}