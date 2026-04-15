using System;

namespace Marketplace.BusinessLogic.Proxy
{
    /// <summary>
    /// PROXY PATTERN — Protection Proxy (ProxySubject).
    /// Controlează accesul la RealProductService în funcție de rolul utilizatorului.
    ///
    /// Reguli de acces:
    ///   - Produse non-premium → accesibile tuturor (inclusiv Guest)
    ///   - Produse premium     → accesibile doar utilizatorilor cu rol "Admin" sau "PremiumUser"
    ///   - Preț premium        → ascuns pentru utilizatorii neautentificați (Guest)
    ///
    /// Clientul (controller, test) vorbește DOAR cu această interfață —
    /// nu știe că există un proxy în față.
    /// </summary>
    public class ProductAccessProxy : IProtectedProductService
    {
        private readonly IProtectedProductService _realService;

        public ProductAccessProxy(IProtectedProductService? realService = null)
        {
            _realService = realService ?? new RealProductService();
        }

        public ProductDetails? GetProductDetails(int productId, string? userRole = null)
        {
            // Obținem produsul din serviciul real pentru a verifica dacă e premium
            var product = _realService.GetProductDetails(productId);

            if (product == null) return null;

            // Verificare acces pentru produse premium
            if (product.IsPremium)
            {
                bool hasAccess = userRole is "Admin" or "PremiumUser";
                if (!hasAccess)
                {
                    Console.WriteLine($"[🔒 PROXY] Acces REFUZAT la produsul premium #{productId}" +
                                      $" pentru rolul '{userRole ?? "Guest"}'.");
                    // Returnăm versiunea "mascată" — fără detalii sensibile
                    return product with
                    {
                        Description = "⚠ Acces restricționat. Actualizați abonamentul pentru detalii.",
                        Price = 0m
                    };
                }

                Console.WriteLine($"[✅ PROXY] Acces ACORDAT la produsul premium #{productId}" +
                                  $" pentru rolul '{userRole}'.");
            }

            return product;
        }

        public decimal GetProductPrice(int productId, string? userRole = null)
        {
            // Utilizatorii neautentificați (Guest) nu văd prețul produselor premium
            var product = _realService.GetProductDetails(productId);
            if (product == null) return 0m;

            if (product.IsPremium && userRole is null or "Guest")
            {
                Console.WriteLine($"[🔒 PROXY] Preț ascuns pentru produsul premium #{productId}" +
                                  $" — utilizator neautentificat.");
                return -1m; // Semnal că prețul este restricționat
            }

            return _realService.GetProductPrice(productId, userRole);
        }

        public bool IsProductAvailable(int productId)
        {
            // Disponibilitatea e publică — fără restricții
            return _realService.IsProductAvailable(productId);
        }
    }
}
