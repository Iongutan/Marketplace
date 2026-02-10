using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Factories
{
    // Creator
    public abstract class ProductFactory
    {


        // Factory Method
        public abstract Product CreateProduct(string name, decimal price, int stock);
    }
}
