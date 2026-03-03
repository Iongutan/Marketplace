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
        /// <summary>Construiește catalogul complet al marketplace-ului.</summary>
        public CatalogCategory BuildFullCatalog()
        {
            var root = new CatalogCategory("Catalog Marketplace", "Toate produsele disponibile");

            // ─── Produse Fizice ──────────────────────────────────────────────────
            var physical = new CatalogCategory("Produse Fizice", "Gadgeturi și mobilier");

            var laptops = new CatalogCategory("Laptopuri");
            laptops.Add(new CatalogProduct(1, "Premium Laptop Pro", 12999m, "Dell", "Intel i7, 16GB RAM, 512GB SSD"));
            laptops.Add(new CatalogProduct(2, "UltraBook Slim", 9499m, "Lenovo", "Intel i5, 8GB RAM, 256GB SSD"));

            var phones = new CatalogCategory("Telefoane Mobile");
            phones.Add(new CatalogProduct(3, "Smartphone NextGen X", 7999m, "Samsung", "AMOLED 6.7\", 256GB"));
            phones.Add(new CatalogProduct(4, "ProPhone 15", 8999m, "Apple", "iOS, 128GB, camera 48MP"));

            var furniture = new CatalogCategory("Mobilier");
            furniture.Add(new CatalogProduct(9, "Minimalist Ergonomic Chair", 3499m, "IKEA", "Reglabil, mesh respirabil"));
            furniture.Add(new CatalogProduct(10, "Standing Desk Pro", 4999m, "FlexiSpot", "Electrică, 140x70cm"));

            physical.Add(laptops);
            physical.Add(phones);
            physical.Add(furniture);

            // ─── Produse Digitale ────────────────────────────────────────────────
            var digital = new CatalogCategory("Produse Digitale", "Licențe, cursuri și e-books");

            var courses = new CatalogCategory("Cursuri Digitale");
            courses.Add(new CatalogProduct(5, "FullStack Web Development", 599m, "TechAcademy",
                                           "React + .NET + SQL", isDigital: true));
            courses.Add(new CatalogProduct(6, "Design Patterns in C#", 399m, "CodeMaster",
                                           "GoF patterns aplicate în .NET", isDigital: true));

            var ebooks = new CatalogCategory("E-Books");
            ebooks.Add(new CatalogProduct(7, "Mastering Design Patterns", 149m, "OReilly",
                                          "PDF + EPUB", isDigital: true));
            ebooks.Add(new CatalogProduct(8, "Clean Code in Depth", 129m, "Pragmatic",
                                          "PDF", isDigital: true));

            digital.Add(courses);
            digital.Add(ebooks);

            // ─── Interior & Mobilier (Legacy handled in physical) ─────────────

            // ─── Asamblare catalog ───────────────────────────────────────────────
            root.Add(physical);
            root.Add(digital);

            return root;
        }

        /// <summary>Construiește un catalog mic pentru teste rapide.</summary>
        public CatalogCategory BuildSimpleCatalog()
        {
            var cat = new CatalogCategory("Demo Catalog");
            cat.Add(new CatalogProduct(1, "Laptop Demo", 5000m, "Dell"));
            cat.Add(new CatalogProduct(2, "Curs .NET", 300m, "Udemy", isDigital: true));
            cat.Add(new CatalogProduct(3, "Scaun Office", 1200m, "IKEA"));
            return cat;
        }
    }
}
