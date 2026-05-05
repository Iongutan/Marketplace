using System;

namespace Marketplace.BusinessLogic.Strategy
{
    public interface IPricingStrategy
    {
        decimal CalculatePrice(decimal basePrice);
    }

    public class RegularPricingStrategy : IPricingStrategy
    {
        public decimal CalculatePrice(decimal basePrice)
        {
            return basePrice;
        }
    }

    public class HolidayDiscountStrategy : IPricingStrategy
    {
        public decimal CalculatePrice(decimal basePrice)
        {
            // 20% reducere
            return basePrice * 0.8m;
        }
    }

    public class ClearanceStrategy : IPricingStrategy
    {
        public decimal CalculatePrice(decimal basePrice)
        {
            // 50% reducere
            return basePrice * 0.5m;
        }
    }

    public class PriceCalculatorContext
    {
        private IPricingStrategy _strategy;

        public PriceCalculatorContext(IPricingStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IPricingStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal Calculate(decimal basePrice)
        {
            return _strategy.CalculatePrice(basePrice);
        }
    }
}
