using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Product> GetProductsByUserId(int userId)
        {
            return _productRepository.GetAll().Where(p => p.UserId == userId);
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public void AddProduct(Product product)
        {
            _productRepository.Insert(product);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }
    }
}
