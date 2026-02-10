using System;

namespace Marketplace.BusinessLogic.AbstractFactory
{
    // Concrete Product A2
    public class EmailShipping : IShippingMethod
    {
        public void Ship(object order)
        {
            Console.WriteLine($"[Digital] Sending download link via Email.");
        }
    }



    // Concrete Factory 2
    public class DigitalOrderFactory : IOrderFactory
    {
        public IShippingMethod CreateShippingMethod()
        {
            return new EmailShipping();
        }

        public IPaymentMethod CreatePaymentMethod()
        {
            return new CardPayment(); // Changed from PayPal to Card as requested
        }
    }
}
