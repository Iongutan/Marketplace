using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Builders
{
    public class ProductDirector
    {
        public Product ConstructLaptop(IProductBuilder builder)
        {
            return builder.Reset(isDigital: false)
                          .SetName("Premium Laptop Pro")
                          .SetDescription("High-end workstation for intensive professionals")
                          .SetPrice(2500.00m)
                          .SetCategory("Produse Electronice")
                          .SetBrand("Marketplace")
                          .SetStock(10)
                          .SetPhysicalDetails(weight: 2.5m, shippingCost: 45.0m)
                          .Build();
        }

        public Product ConstructEBook(IProductBuilder builder)
        {
            return builder.Reset(isDigital: true)
                          .SetName("Mastering Design Patterns")
                          .SetDescription("Deep dive into advanced architectual patterns")
                          .SetPrice(49.99m)
                          .SetCategory("Produse Online")
                          .SetBrand("DevPress")
                          .SetStock(1000)
                          .SetDigitalDetails("https://dl.marketplace.com/ebook-123", "EPUB/PDF")
                          .Build();
        }
    }
}
