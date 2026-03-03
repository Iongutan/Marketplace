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
        var products = _productService.GetProducts()
                                      .Take(settings.MaxProductsPerUser); // Limit for demonstration as requested

        if (!string.IsNullOrEmpty(category))
        {
            if (category == "Produse Digitale")
            {
                products = products.Where(p => p.IsDigital == true);
            }
            else if (category == "Produse Fizice")
            {
                products = products.Where(p => p.IsDigital == false);
            }
            else if (category == "Interior")
            {
                products = products.Where(p => p.Category == "Interior" || p.Category == "Mobilă");
            }
            else
            {
                products = products.Where(p => p.Category == category);
            }
        }

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
