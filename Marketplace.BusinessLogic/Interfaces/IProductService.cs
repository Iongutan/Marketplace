using System.Collections.Generic;
using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductsByUserId(int userId);
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
