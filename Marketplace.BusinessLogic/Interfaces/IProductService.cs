using System.Collections.Generic;
using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
    }
}
