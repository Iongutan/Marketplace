using System;

namespace Marketplace.BusinessLogic.AbstractFactory
{
    // Concrete Product A1
    public class CourierShipping : IShippingMethod
    {
        public void Ship(object order)
        {
            Console.WriteLine($"[Physical] Shipping order via Courier. Tracking number generated.");
        }
    }

    // Concrete Product B1 (Option 1)
    public class CardPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"[Physical] Processing credit card payment of {amount:C}.");
        }
    }

    // Concrete Product B1 (Option 2) - NEW
    public class CashPayment : IPaymentMethod
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"[Physical] Collecting cash payment of {amount:C} via Courier.");
        }
    }

    // Concrete Factory 1
    public class PhysicalOrderFactory : IOrderFactory
    {
        private readonly string _paymentType;

        public PhysicalOrderFactory(string paymentType = "Card")
        {
            _paymentType = paymentType;
        }

        public IShippingMethod CreateShippingMethod()
        {
            return new CourierShipping();
        }

        public IPaymentMethod CreatePaymentMethod()
        {
            if (_paymentType == "Cash")
            {
                return new CashPayment();
            }
            return new CardPayment();
        }
    }
}
