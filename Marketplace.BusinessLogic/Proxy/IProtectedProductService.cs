namespace Marketplace.BusinessLogic.Proxy
{
    /// <summary>
    /// PROXY PATTERN — Interfața subiectului (Subject).
    /// Atât serviciul real (RealProductService) cât și proxy-ul
    /// (ProductAccessProxy) implementează această interfață,
    /// astfel clientul nu știe cu ce lucrează de fapt.
    /// </summary>
    public interface IProtectedProductService
    {
        /// <summary>
        /// Returnează detaliile complete ale unui produs.
        /// Proxy-ul poate restricționa accesul pentru produse premium.
        /// </summary>
        ProductDetails? GetProductDetails(int productId, string? userRole = null);

        /// <summary>
        /// Returnează prețul unui produs.
        /// Proxy-ul poate ascunde prețul pentru utilizatorii neautentificați.
        /// </summary>
        decimal GetProductPrice(int productId, string? userRole = null);

        /// <summary>Verifică disponibilitatea unui produs.</summary>
        bool IsProductAvailable(int productId);
    }

    /// <summary>DTO cu detaliile unui produs.</summary>
    public record ProductDetails(
        int Id,
        string Name,
        decimal Price,
        string Category,
        string Brand,
        int Stock,
        bool IsPremium,
        string? Description = null
    );
}
