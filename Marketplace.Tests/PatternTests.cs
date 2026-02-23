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
            Assert.Equal(original.Name, clone.Name);
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
            Assert.Equal("Digital", ebook.Category);
        }
    }
}
