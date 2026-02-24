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

        public Product ConstructSmartphone(IProductBuilder builder)
        {
            return builder.Reset(isDigital: false)
                          .SetName("Smartphone NextGen X")
                          .SetDescription("Latest flagship smartphone with amazing camera")
                          .SetPrice(999.00m)
                          .SetCategory("Produse Electronice")
                          .SetBrand("Marketplace")
                          .SetStock(50)
                          .SetPhysicalDetails(weight: 0.2m, shippingCost: 15.0m)
                          .Build();
        }

        public Product ConstructCourse(IProductBuilder builder)
        {
            return builder.Reset(isDigital: true)
                          .SetName("FullStack Web Development")
                          .SetDescription("Learn everything from HTML to Cloud Deployment")
                          .SetPrice(199.99m)
                          .SetCategory("Produse Online")
                          .SetBrand("EduMarket")
                          .SetStock(5000)
                          .SetDigitalDetails("https://dl.marketplace.com/course-456", "VIDEO/ZIP")
                          .Build();
        }

        public Product ConstructFurniture(IProductBuilder builder)
        {
            return builder.Reset(isDigital: false)
                          .SetName("Minimalist Ergonomic Chair")
                          .SetDescription("Comfortable chair for long working hours")
                          .SetPrice(350.00m)
                          .SetCategory("Interior")
                          .SetBrand("HomeLux")
                          .SetStock(5)
                          .SetPhysicalDetails(weight: 12.5m, shippingCost: 60.0m)
                          .Build();
        }
    }
}
