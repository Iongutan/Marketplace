using Xunit;
using Marketplace.Domain.Entities;
using Marketplace.BusinessLogic.Builders;
using Marketplace.BusinessLogic.Singletons;
using System;

namespace Marketplace.Tests
{
    public class PatternTests
    {
        [Fact]
        public void Singleton_ShouldReturnSameInstance()
        {
            // Arrange & Act
            var instance1 = MarketplaceSettings.Instance;
            var instance2 = MarketplaceSettings.Instance;

            // Assert
            Assert.Same(instance1, instance2);
            Assert.Equal("Premium Tech Marketplace", instance1.SiteName);
        }

        [Fact]
        public void Prototype_ShouldCloneCorrectly()
        {
            // Arrange
            var original = new Product
            {
                Name = "Original",
                Price = 100m,
                Category = "Test",
                Brand = "TestBrand"
            };

            // Act
            var clone = original.Clone();

            // Assert
            Assert.NotSame(original, clone);
            Assert.Equal("Copie a " + original.Name, clone.Name);
            Assert.Equal(original.Price, clone.Price);
            Assert.Equal(original.Category, clone.Category);
        }

        [Fact]
        public void Builder_ShouldConstructProductStepByStep()
        {
            // Arrange
            var builder = new ProductBuilder();

            var product = builder.Reset(isDigital: false)
                                 .SetName("Custom Phone")
                                 .SetPrice(500m)
                                 .SetStock(5)
                                 .SetPhysicalDetails(weight: 0.2m, shippingCost: 10m)
                                 .Build();

            // Assert
            Assert.Equal("Custom Phone", product.Name);
            Assert.Equal(500m, product.Price);
            Assert.Equal(5, product.Stock);
            Assert.False(product.IsDigital);
        }

        [Fact]
        public void Director_ShouldConstructPresetProducts()
        {
            // Arrange
            var builder = new ProductBuilder();
            var director = new ProductDirector();

            // Act
            var laptop = director.ConstructLaptop(builder);
            var ebook = director.ConstructEBook(builder);

            // Assert
            Assert.Equal("Premium Laptop Pro", laptop.Name);
            Assert.True(ebook.IsDigital);
            Assert.Equal("Produse Digitale", ebook.Category);
        }
        [Fact]
        public void Prototype_ShouldCloneAsTemplateCorrectly()
        {
            // Arrange
            var original = new Product { Name = "Original", Brand = "Red", Stock = 10, Category = "Test" };

            // Act
            var template = original.CloneAsTemplate();

            // Assert
            Assert.Equal(original.Name, template.Name); // Name should persist in template
            Assert.Equal(0, template.Stock); // Stock should be reset
            Assert.Equal(original.Category, template.Category);
            Assert.Equal(0, template.Id);
        }

        [Fact]
        public void Director_ShouldConstructNewSpecializedProducts()
        {
            // Arrange
            var builder = new ProductBuilder();
            var director = new ProductDirector();

            // Act
            var phone = director.ConstructSmartphone(builder);
            var course = director.ConstructCourse(builder);
            var chair = director.ConstructFurniture(builder);

            // Assert
            Assert.Equal("Smartphone NextGen X", phone.Name);
            Assert.False(phone.IsDigital);

            Assert.Equal("FullStack Web Development", course.Name);
            Assert.True(course.IsDigital);

            Assert.Equal("Minimalist Ergonomic Chair", chair.Name);
            Assert.Equal("Interior", chair.Category);
        }
        [Fact]
        public void Polymorphism_ShouldReturnSpecificDetails()
        {
            // Arrange
            Product digital = new DigitalProduct { Name = "EBook", Price = 10, Stock = 100, DownloadUrl = "http://test.com" };
            Product physical = new PhysicalProduct { Name = "Laptop", Price = 1000, Stock = 5, ShippingCost = 50 };

            // Act
            string digitalDetails = digital.GetEntityDetails();
            string physicalDetails = physical.GetEntityDetails();

            // Assert
            Assert.Contains("Digital Product", digitalDetails);
            Assert.Contains("http://test.com", digitalDetails);
            Assert.Contains("Physical Product", physicalDetails);
            Assert.Contains("Shipping", physicalDetails);
        }
        [Fact]
        public void Security_PasswordShouldBeHashed()
        {
            // Arrange
            var user = new User { Username = "test", Password = "plainPassword", Email = "test@test.com" };
            var mockRepo = new Moq.Mock<Marketplace.Data.Interfaces.IRepository<User>>();
            mockRepo.Setup(r => r.GetAll()).Returns(new List<User>().AsQueryable());
            var userApi = new Marketplace.BusinessLogic.Core.UserApi(mockRepo.Object);

            // Act
            userApi.Register(user);

            // Assert
            Assert.NotEqual("plainPassword", user.Password);
            Assert.True(BCrypt.Net.BCrypt.Verify("plainPassword", user.Password));
        }

        // ═══════════════════════════════════════════════════════════════════════
        //  Lab 4 — Structural Patterns
        // ═══════════════════════════════════════════════════════════════════════

        // ── Adapter ──────────────────────────────────────────────────────────
        [Fact]
        public void Adapter_PayPal_ShouldProcessPaymentSuccessfully()
        {
            // Arrange
            Marketplace.BusinessLogic.Adapter.IPaymentGateway gateway =
                new Marketplace.BusinessLogic.Adapter.PayPalAdapter();

            // Act
            bool result = gateway.ProcessPayment("ORDER-001", 250m, "MDL");

            // Assert
            Assert.True(result);
            Assert.Equal("PayPal", gateway.GatewayName);
        }

        [Fact]
        public void Adapter_Stripe_ShouldProcessPaymentSuccessfully()
        {
            // Arrange
            Marketplace.BusinessLogic.Adapter.IPaymentGateway gateway =
                new Marketplace.BusinessLogic.Adapter.StripeAdapter();

            // Act
            bool result = gateway.ProcessPayment("ORDER-002", 499.99m, "MDL");

            // Assert
            Assert.True(result);
            Assert.Equal("Stripe", gateway.GatewayName);
        }

        [Fact]
        public void Adapter_GooglePay_ShouldProcessPaymentSuccessfully()
        {
            // Arrange
            Marketplace.BusinessLogic.Adapter.IPaymentGateway gateway =
                new Marketplace.BusinessLogic.Adapter.GooglePayAdapter();

            // Act
            bool result = gateway.ProcessPayment("ORDER-003", 99m, "MDL");

            // Assert
            Assert.True(result);
            Assert.Equal("Google Pay", gateway.GatewayName);
        }

        [Fact]
        public void Adapter_AllGatewaysShouldImplementCommonInterface()
        {
            // Adapter Pattern: toate cele 3 clase diferite sunt folosite uniform
            // prin interfața IPaymentGateway — clientul nu știe care e folosit.
            var gateways = new Marketplace.BusinessLogic.Adapter.IPaymentGateway[]
            {
                new Marketplace.BusinessLogic.Adapter.PayPalAdapter(),
                new Marketplace.BusinessLogic.Adapter.StripeAdapter(),
                new Marketplace.BusinessLogic.Adapter.GooglePayAdapter()
            };

            foreach (var gw in gateways)
            {
                bool ok = gw.ProcessPayment("ORDER-TEST", 100m, "MDL");
                Assert.True(ok, $"{gw.GatewayName} should process payment successfully");
                Assert.False(string.IsNullOrEmpty(gw.GatewayName));
            }
        }

        // ── Composite ─────────────────────────────────────────────────────────
        [Fact]
        public void Composite_CatalogShouldCountProductsRecursively()
        {
            // Arrange — construim un catalog simplu
            var catalogService = new Marketplace.BusinessLogic.Composite.CatalogService();
            var catalog = catalogService.BuildSimpleCatalog();

            // Act
            int count = catalog.GetProductCount();
            var allProducts = catalog.GetAllProducts().ToList();

            // Assert
            Assert.Equal(3, count);
            Assert.Equal(3, allProducts.Count);
        }

        [Fact]
        public void Composite_CatalogCategoryShouldTreatChildrenUniformly()
        {
            // Composite Pattern: CatalogCategory și CatalogProduct
            // sunt tratate uniform prin ICatalogComponent
            var root = new Marketplace.BusinessLogic.Composite.CatalogCategory("Test Root");

            // Adăugăm un leaf direct
            root.Add(new Marketplace.BusinessLogic.Composite.CatalogProduct(1, "Laptop", 5000m, "Dell"));

            // Adăugăm un composite (subcategorie cu 2 produse)
            var subCat = new Marketplace.BusinessLogic.Composite.CatalogCategory("Software");
            subCat.Add(new Marketplace.BusinessLogic.Composite.CatalogProduct(2, "Windows 11", 699m, isDigital: true));
            subCat.Add(new Marketplace.BusinessLogic.Composite.CatalogProduct(3, "Office 365", 499m, isDigital: true));
            root.Add(subCat);

            // Assert — GetProductCount() funcționează recursiv
            Assert.Equal(3, root.GetProductCount());
            Assert.Equal(2, subCat.GetProductCount());

            // Toate produsele sunt recuperate uniform din ierarhie
            var products = root.GetAllProducts().ToList();
            Assert.Equal(3, products.Count);
        }

        [Fact]
        public void Composite_FullCatalogShouldHaveCorrectStructure()
        {
            // Arrange
            var catalogService = new Marketplace.BusinessLogic.Composite.CatalogService();
            var fullCatalog = catalogService.BuildFullCatalog();

            // Assert
            Assert.NotEmpty(fullCatalog.GetChildren());
            Assert.True(fullCatalog.GetProductCount() > 0);
            // Catalogul complet are cel puțin 8 produse (câte am definit în BuildFullCatalog)
            Assert.True(fullCatalog.GetProductCount() >= 8);
        }

        // ── Façade ────────────────────────────────────────────────────────────
        [Fact]
        public void Facade_OrderFacadeShouldPlaceOrderSuccessfully()
        {
            // Arrange
            var facade = new Marketplace.BusinessLogic.Facade.OrderFacade();

            // Act — o singură metodă înlocuiește 4 pași manuali (Façade Pattern)
            var result = facade.PlaceOrder(
                productId: 1,
                productName: "Premium Laptop Pro",
                pricePerUnit: 12999m,
                isDigital: false,
                sellerId: 2,
                buyerName: "Ion Popescu",
                buyerEmail: "ion@test.md",
                quantity: 1,
                paymentGateway: "Stripe"
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Ion Popescu", result!.BuyerName);
            Assert.Equal("Premium Laptop Pro", result.ProductName);
            Assert.Equal(12999m, result.TotalAmount);
            Assert.Equal("Stripe", result.PaymentGateway);
            Assert.False(string.IsNullOrEmpty(result.OrderNumber));
            Assert.StartsWith("ORD-", result.OrderNumber);
            Assert.False(string.IsNullOrEmpty(result.TransactionId));
            Assert.StartsWith("MKT-", result.TransactionId);
        }

        [Fact]
        public void Facade_OrderFacadeShouldCalculateTotalCorrectly()
        {
            // Arrange
            var facade = new Marketplace.BusinessLogic.Facade.OrderFacade();
            decimal pricePerUnit = 399m;
            int quantity = 3;

            // Act
            var result = facade.PlaceOrder(
                productId: 5,
                productName: "Design Patterns Course",
                pricePerUnit: pricePerUnit,
                isDigital: true,
                sellerId: 1,
                buyerName: "Maria Test",
                buyerEmail: "maria@test.md",
                quantity: quantity,
                paymentGateway: "PayPal"
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pricePerUnit * quantity, result!.TotalAmount);
            Assert.Equal(quantity, result.Quantity);
            Assert.True(result.IsDigital);
        }
    }
}
