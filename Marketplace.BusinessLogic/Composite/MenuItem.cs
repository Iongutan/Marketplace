using System;
using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Composite
{
    /// <summary>
    /// Leaf (frunză) în Composite Pattern.
    /// Reprezintă un produs individual din catalog marketplace.
    /// Nu conține alte componente.
    /// </summary>
    public class CatalogProduct : ICatalogComponent
    {
        public string Name { get; }
        public string? Description { get; }
        public decimal Price { get; }
        public string? Brand { get; }
        public bool IsDigital { get; }
        public int ProductId { get; }

        public CatalogProduct(int id, string name, decimal price,
                              string? brand = null, string? description = null,
                              bool isDigital = false)
        {
            ProductId = id;
            Name = name;
            Price = price;
            Brand = brand;
            Description = description;
            IsDigital = isDigital;
        }

        public void Display(int depth = 0)
        {
            string indent = new string(' ', depth * 2);
            string type = IsDigital ? "[Digital]" : "[Fizic]";
            string brandStr = string.IsNullOrEmpty(Brand) ? "" : $" | {Brand}";
            Console.WriteLine($"{indent}  {type} {Name}{brandStr} — {Price:F2} MDL");
        }

        public int GetProductCount() => 1;

        public IEnumerable<CatalogProduct> GetAllProducts()
        {
            yield return this;
        }
    }
}
