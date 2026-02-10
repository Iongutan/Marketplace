using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Factories
{
    // Concrete Creator for Physical Products
    public class PhysicalProductFactory : ProductFactory
    {
        public override Product CreateProduct(string name, decimal price, int stock)
        {
            return new PhysicalProduct
            {
                Name = name,
                Price = price,
                Stock = stock,
                Weight = 1.0m, // Default
                ShippingCost = 10.0m // Default
            };
        }
    }
}
