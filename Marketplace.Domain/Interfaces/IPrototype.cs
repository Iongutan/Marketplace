namespace Marketplace.Domain.Interfaces
{
    public interface IPrototype<T>
    {
        T Clone();
    }
}
