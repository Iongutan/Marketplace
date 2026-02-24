using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Factories
{
    // Concrete Creator for Digital Products
    public class DigitalProductFactory : ProductFactory
    {
        public override Product CreateProduct(string name, decimal price, int stock)
        {
            return new DigitalProduct
            {
                Name = name,
                Price = price,
                Stock = stock,
                DownloadUrl = "http://example.com/download", // Default
                FileFormat = "PDF",
                IsDigital = true
            };
        }
    }
}
