using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Builders
{
    public class ProductBuilder : IProductBuilder
    {
        private Product _product = new Product();

        public IProductBuilder Reset(bool isDigital)
        {
            _product = isDigital ? new DigitalProduct { IsDigital = true, Category = "Digital" } : new PhysicalProduct { IsDigital = false };
            return this;
        }

        public IProductBuilder SetName(string name)
        {
            _product.Name = name;
            return this;
        }

        public IProductBuilder SetDescription(string description)
        {
            _product.Description = description;
            return this;
        }

        public IProductBuilder SetPrice(decimal price)
        {
            _product.Price = price;
            return this;
        }

        public IProductBuilder SetCategory(string category)
        {
            _product.Category = category;
            return this;
        }

        public IProductBuilder SetBrand(string brand)
        {
            _product.Brand = brand;
            return this;
        }

        public IProductBuilder SetStock(int stock)
        {
            _product.Stock = stock;
            return this;
        }

        public IProductBuilder SetPhysicalDetails(decimal weight, decimal shippingCost)
        {
            if (_product is PhysicalProduct physical)
            {
                physical.Weight = weight;
                physical.ShippingCost = shippingCost;
            }
            return this;
        }

        public IProductBuilder SetDigitalDetails(string downloadUrl, string fileFormat)
        {
            if (_product is DigitalProduct digital)
            {
                digital.DownloadUrl = downloadUrl;
                digital.FileFormat = fileFormat;
            }
            return this;
        }

        public Product Build()
        {
            Product result = _product;
            _product = new Product(); // Temporary neutral state
            return result;
        }
    }
}
