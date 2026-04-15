namespace Marketplace.BusinessLogic.Flyweight
{
    /// <summary>
    /// FLYWEIGHT PATTERN — Obiectul Flyweight concret.
    /// Conține STAREA INTRINSECĂ: brand, categorie, tipul de produs și expediere.
    /// Aceste proprietăți sunt IMUTABILE și pot fi partajate între mii de produse.
    ///
    /// Exemplu marketplace: dacă avem 500 de laptopuri Dell din categoria "Electronics",
    /// toate vor referi ACEEAȘI instanță ProductFlyweight("Dell","Electronics",false,"Standard").
    /// </summary>
    public class ProductFlyweight : IProductFlyweight
    {
        public string Brand { get; }
        public string Category { get; }
        public bool IsDigital { get; }
        public string ShippingType { get; }

        internal ProductFlyweight(string brand, string category, bool isDigital, string shippingType)
        {
            Brand = brand;
            Category = category;
            IsDigital = isDigital;
            ShippingType = shippingType;
        }

        /// <summary>
        /// Returnează o descriere a stării intrinseci partajate.
        /// Starea extrinsecă (Id, Name, Price, Stock) NU este stocată aici.
        /// </summary>
        public string GetIntrinsicState()
            => $"Brand={Brand} | Category={Category} | Digital={IsDigital} | Shipping={ShippingType}";
    }
}
