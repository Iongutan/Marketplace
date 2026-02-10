using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.Domain.Entities;

namespace Marketplace.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetProducts();
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
