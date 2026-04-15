namespace Marketplace.BusinessLogic.Flyweight
{
    /// <summary>
    /// FLYWEIGHT PATTERN — Interfața stării intrinseci a unui produs.
    /// Starea intrinsecă = proprietăți care NU se schimbă între produse similare:
    ///   Brand, Category, IsDigital, ShippingType.
    /// Starea extrinsecă (Id, Name, Price, Stock) rămâne la client.
    /// </summary>
    public interface IProductFlyweight
    {
        string Brand { get; }
        string Category { get; }
        bool IsDigital { get; }
        string ShippingType { get; }

        /// <summary>Afișează starea intrinsecă a flyweight-ului.</summary>
        string GetIntrinsicState();
    }
}
