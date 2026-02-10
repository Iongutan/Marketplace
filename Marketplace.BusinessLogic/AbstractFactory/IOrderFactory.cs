namespace Marketplace.BusinessLogic.AbstractFactory
{
    // Abstract Product A
    public interface IShippingMethod
    {
        void Ship(object order);
    }

    // Abstract Product B
    public interface IPaymentMethod
    {
        void ProcessPayment(decimal amount);
    }

    // Abstract Factory
    public interface IOrderFactory
    {
        IShippingMethod CreateShippingMethod();
        IPaymentMethod CreatePaymentMethod();
    }
}
