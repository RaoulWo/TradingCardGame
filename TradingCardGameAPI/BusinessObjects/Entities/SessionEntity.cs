using BusinessObjects.Base;
using BusinessObjects.Interfaces;

namespace BusinessObjects.Entities;

public class SessionEntity : Entity, IAggregateRoot
{
    public Guid PlayerId { get; set; }

    protected override void Validate()
    {
        throw new NotImplementedException();
    }
}