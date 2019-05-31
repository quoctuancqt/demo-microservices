namespace Core.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    public interface IEntity : IEntity<string>
    {

    }
}
