using System.Linq;
using Marketplace.BusinessLogic.Interfaces;

namespace Marketplace.BusinessLogic.Composite
{
    /// <summary>
    /// Serviciu care construiește catalogul ierarhic al marketplace-ului.
    /// Demonstrează Composite Pattern:
    ///   Catalog Root > Categorii > Subcategorii > Produse individuale
    /// Atât CatalogCategory cât și CatalogProduct implementează ICatalogComponent
    /// și pot fi tratate uniform.
    /// </summary>
    public class CatalogService
    {
        private readonly IProductService _productService;

        public CatalogService(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>Construiește catalogul complet al marketplace-ului din baza de date.</summary>
        public CatalogCategory BuildFullCatalog()
        {
            var root = new CatalogCategory("Catalog Marketplace", "");

            try
            {
                var products = _productService.GetProducts() ?? Enumerable.Empty<Marketplace.Domain.Entities.Product>();

                // Definim categoriile fixe
                var physicalCategory = new CatalogCategory("Produse Fizice");
                var digitalCategory = new CatalogCategory("Produse Digitale");
                var interiorCategory = new CatalogCategory("Interior");

                foreach (var p in products)
                {
                    CatalogCategory? target = null;

                    // 1. Digital are prioritate
                    if (p.IsDigital == true)
                    {
                        target = digitalCategory;
                    }
                    // 2. Mapare specială pentru Interior/Mobilă
                    else if (p.Category == "Interior" || p.Category == "Mobilă")
                    {
                        target = interiorCategory;
                    }
                    // 3. Toate celelalte fizice
                    else if (p.IsDigital == false)
                    {
                        target = physicalCategory;
                    }

                    if (target != null)
                    {
                        target.Add(new CatalogProduct(
                            id: p.Id,
                            name: p.Name ?? "Produs fără nume",
                            price: p.Price ?? 0m,
                            brand: p.Brand,
                            description: p.Description,
                            isDigital: p.IsDigital ?? false,
                            stock: p.Stock ?? 0
                        ));
                    }
                }

                // Adăugăm categoriile doar dacă au produse
                if (physicalCategory.GetProductCount() > 0) root.Add(physicalCategory);
                if (digitalCategory.GetProductCount() > 0) root.Add(digitalCategory);
                if (interiorCategory.GetProductCount() > 0) root.Add(interiorCategory);
            }
            catch
            {
                // Fallback la catalog gol în caz de eroare SQL
                root.Add(new CatalogCategory("Eroare", "Nu s-au putut încărca produsele."));
            }

            return root;
        }

        /// <summary>Construiește un catalog mic pentru teste rapide.</summary>
        public CatalogCategory BuildSimpleCatalog()
        {
            var cat = new CatalogCategory("Demo Catalog");
            cat.Add(new CatalogProduct(1, "Produs Test Database", 100m, "SQL Test", stock: 10));
            cat.Add(new CatalogProduct(2, "Produs Test 2", 200m, "SQL Test", stock: 20));
            cat.Add(new CatalogProduct(3, "Produs Test 3", 300m, "SQL Test", stock: 30));
            return cat;
        }
    }
}
