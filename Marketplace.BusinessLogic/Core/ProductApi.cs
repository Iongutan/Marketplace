using System.Collections.Generic;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.Data.Interfaces;
using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Core
{
    public class ProductApi : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductApi(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public void AddProduct(Product product)
        {
            _productRepository.Insert(product);
        }
    }
}
