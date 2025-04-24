using HireHive.Domain.Entities.ValueObjects;

namespace HireHive.Domain.Entities;

public class BaseEntity<T>
{
    public T? Id { get; private set; }
    public Audit Audit { get; private set; }
    public string? ExtraData { get; private set; }

    protected BaseEntity()
    {
        Audit = new Audit();
        ExtraData = string.Empty;
    }

    public virtual void Update()
    {
        Audit.UpdatedAt = DateTime.UtcNow;
    }
}
