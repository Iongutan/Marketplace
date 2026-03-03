using System;

namespace Marketplace.Domain.Entities
{
    public class PhysicalProduct : Product
    {
        public override string GetEntityDetails()
        {
            return $"Physical Product: {Name} - Price: {Price:C} (Stock: {Stock}) - Shipping: {ShippingCost:C}";
        }

        public decimal Weight { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
