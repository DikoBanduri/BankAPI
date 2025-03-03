namespace Bank.Service.Interfaces.Repository
{
    public interface IEntity<out T>
    {
        T Id { get; }
    }

    public interface IEntity : IEntity<int>
    {

    }
}
