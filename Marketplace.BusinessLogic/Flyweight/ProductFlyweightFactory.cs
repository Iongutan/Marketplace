using System;
using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Flyweight
{
    /// <summary>
    /// FLYWEIGHT PATTERN — Factory (FlyweightFactory).
    /// Menține un cache intern (pool) de instanțe ProductFlyweight.
    /// Cheile sunt combinații unice: "Brand|Category|IsDigital|ShippingType".
    ///
    /// Impact: În loc să creăm 1000 de obiecte identice pentru 1000 de laptopuri Dell,
    /// creăm UN SINGUR flyweight și îl partajăm — memoria scade dramatic.
    /// </summary>
    public class ProductFlyweightFactory
    {
        private readonly Dictionary<string, ProductFlyweight> _cache = new();

        /// <summary>
        /// Returnează un flyweight existent din cache, sau creează unul nou.
        /// Cheia este hash-ul combinat al stării intrinseci.
        /// </summary>
        public IProductFlyweight GetFlyweight(string brand, string category,
                                               bool isDigital, string shippingType = "Standard")
        {
            string key = BuildKey(brand, category, isDigital, shippingType);

            if (!_cache.TryGetValue(key, out var flyweight))
            {
                flyweight = new ProductFlyweight(brand, category, isDigital, shippingType);
                _cache[key] = flyweight;
                Console.WriteLine($"[Flyweight] ✦ Creat FlyWeight NOU → {key}");
            }
            else
            {
                Console.WriteLine($"[Flyweight] ✓ Reutilizat flyweight existent → {key}");
            }

            return flyweight;
        }

        /// <summary>Numărul de flyweight-uri unice stocate în cache.</summary>
        public int CachedCount => _cache.Count;

        /// <summary>Listează toate flyweight-urile din cache.</summary>
        public IReadOnlyDictionary<string, ProductFlyweight> Cache => _cache;

        private static string BuildKey(string brand, string category, bool isDigital, string shippingType)
            => $"{brand}|{category}|{isDigital}|{shippingType}";
    }
}
