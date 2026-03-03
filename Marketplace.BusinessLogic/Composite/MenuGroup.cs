using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.BusinessLogic.Composite
{
    /// <summary>
    /// Composite (nod intern) în Composite Pattern.
    /// Reprezintă o categorie (sau subcategorie) din catalogul marketplace.
    /// Poate conține atât CatalogProduct (leafs) cât și alte CatalogCategory (composites).
    /// </summary>
    public class CatalogCategory : ICatalogComponent
    {
        private readonly List<ICatalogComponent> _children = new();

        public string Name { get; }
        public string? Description { get; }

        public CatalogCategory(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }

        // ─── Gestiunea copiilor (add/remove/get) ────────────────────────────────
        public void Add(ICatalogComponent component) => _children.Add(component);
        public void Remove(ICatalogComponent component) => _children.Remove(component);
        public IReadOnlyList<ICatalogComponent> GetChildren() => _children.AsReadOnly();

        public void Display(int depth = 0)
        {
            string indent = new string(' ', depth * 2);
            int count = GetProductCount();
            Console.WriteLine($"{indent}[Categorie] {Name} ({count} produse)" +
                              (string.IsNullOrEmpty(Description) ? "" : $" — {Description}"));
            foreach (var child in _children)
                child.Display(depth + 1);
        }

        public int GetProductCount() => _children.Sum(c => c.GetProductCount());

        public IEnumerable<CatalogProduct> GetAllProducts()
            => _children.SelectMany(c => c.GetAllProducts());
    }
}
