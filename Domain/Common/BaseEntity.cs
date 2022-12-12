namespace CleanArc.Domain.Common;

public interface IEntity
{
}

public interface ITimeModification
{
    DateTime CreatedTime { get; set; }
    DateTime? ModifiedDate { get; set; }
}

public abstract class BaseEntity<TKey> : IEntity, ITimeModification
{
    public TKey Id { get; protected set; }

    public override bool Equals(object obj)
    {
        if (!(obj is BaseEntity<TKey> other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(BaseEntity<TKey> a, BaseEntity<TKey> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity<TKey> a, BaseEntity<TKey> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    public DateTime CreatedTime { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public abstract class BaseEntity : BaseEntity<int>
{

}