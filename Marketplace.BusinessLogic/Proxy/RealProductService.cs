using System;
using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Proxy
{
    /// <summary>
    /// PROXY PATTERN — Serviciul REAL (RealSubject).
    /// Accesează și returnează datele reale ale produselor.
    /// Proxy-ul va delega la această clasă DOAR după verificarea drepturilor.
    /// </summary>
    public class RealProductService : IProtectedProductService
    {
        // Catalog de produse simulate (înlocuiește în producție cu IRepository)
        private readonly Dictionary<int, ProductDetails> _products = new()
        {
            [1]  = new(1,  "Smartphone NextGen X",     2999m,  "Electronics",    "Samsung",  50,  false, "Cel mai nou model cu cameră 200MP"),
            [2]  = new(2,  "Premium Laptop Pro",        12999m, "Electronics",    "Apple",    10,  true,  "MacBook Pro M3 — acces restricționat"),
            [3]  = new(3,  "Design Patterns Course",    399m,   "Produse Digitale","Udemy",   999, false, "Curs complet design patterns C#"),
            [4]  = new(4,  "4K Gaming Monitor",         4999m,  "Electronics",    "LG",       25,  true,  "Monitor premium 144Hz HDR — acces restricționat"),
            [5]  = new(5,  "Minimalist Ergonomic Chair",3499m,  "Interior",       "IKEA",     15,  false, "Scaun ergonomic pentru birou"),
        };

        public ProductDetails? GetProductDetails(int productId, string? userRole = null)
        {
            _products.TryGetValue(productId, out var product);
            Console.WriteLine($"[RealProductService] Acces acordat pentru produsul #{productId}: {product?.Name ?? "negăsit"}");
            return product;
        }

        public decimal GetProductPrice(int productId, string? userRole = null)
        {
            if (_products.TryGetValue(productId, out var product))
            {
                Console.WriteLine($"[RealProductService] Preț produs #{productId}: {product.Price} MDL");
                return product.Price;
            }
            return 0m;
        }

        public bool IsProductAvailable(int productId)
        {
            if (_products.TryGetValue(productId, out var product))
                return product.Stock > 0;
            return false;
        }
    }
}
