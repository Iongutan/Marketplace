using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Web.Models;
using Marketplace.BusinessLogic.Interfaces;
using System.Linq;

namespace Marketplace.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index(string? category, string? brand, string? search)
    {
        var settings = Marketplace.BusinessLogic.Singletons.MarketplaceSettings.Instance;
        var baseProducts = _productService.GetProducts().Take(settings.MaxProductsPerUser);

        // ==== 1. ITERATOR PATTERN (Lab 6) ====
        var collection = new Marketplace.BusinessLogic.Iterator.ProductCollection();
        collection.AddRange(baseProducts);
        var filteredList = new List<Marketplace.Domain.Entities.Product>();

        if (!string.IsNullOrEmpty(category) && category != "Produse Digitale" && category != "Produse Fizice" && category != "Interior")
        {
            // Folosim Category Iterator creat la Lab 6 pentru filtrarea categoriilor reale
            var catIterator = collection.CreateCategoryIterator(category);
            while (catIterator.HasNext())
            {
                filteredList.Add((Marketplace.Domain.Entities.Product)catIterator.Next());
            }
        }
        else
        {
            // Folosim Normal Iterator
            var iterator = collection.CreateIterator();
            while (iterator.HasNext())
            {
                filteredList.Add((Marketplace.Domain.Entities.Product)iterator.Next());
            }

            // Aplicam filtrele manuale vechi pentru meniurile hardcodate
            if (category == "Produse Digitale")
                filteredList = filteredList.Where(p => p.IsDigital == true).ToList();
            else if (category == "Produse Fizice")
                filteredList = filteredList.Where(p => p.IsDigital == false).ToList();
            else if (category == "Interior")
                filteredList = filteredList.Where(p => p.Category == "Interior" || p.Category == "Mobilă").ToList();
        }

        var products = filteredList.AsEnumerable();

        if (!string.IsNullOrEmpty(brand))
        {
            products = products.Where(p => p.Brand != null && p.Brand.Contains(brand, System.StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(search))
        {
            products = products.Where(p =>
                (p.Name != null && p.Name.Contains(search, System.StringComparison.OrdinalIgnoreCase)) ||
                (p.Description != null && p.Description.Contains(search, System.StringComparison.OrdinalIgnoreCase)));
        }

        ViewBag.CurrentCategory = category;
        ViewBag.CurrentBrand = brand;
        ViewBag.CurrentSearch = search;

        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
