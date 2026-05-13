namespace MyRest.Domain.Base;

public abstract class Entity<TId>(TId id) where TId : struct, IEquatable<TId>
{
    public TId Id { get; protected set; } = id;

    protected Entity() : this(default!) { }
}