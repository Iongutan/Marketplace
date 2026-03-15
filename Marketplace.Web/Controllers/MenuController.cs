using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Composite;

namespace Marketplace.Web.Controllers
{
    /// <summary>
    /// Controller demonstrând Composite Pattern:
    /// CatalogCategory și CatalogProduct sunt tratate uniform prin ICatalogComponent.
    /// </summary>
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogService;

        public CatalogController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public IActionResult Index()
        {
            var catalog = _catalogService.BuildFullCatalog();
            return View(catalog);
        }
    }
}
