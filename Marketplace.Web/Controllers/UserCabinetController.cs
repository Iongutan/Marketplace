using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.Domain.Entities;
using System.Security.Claims;
using System.Linq;

namespace Marketplace.Web.Controllers
{
    [Authorize]
    public class UserCabinetController : Controller
    {
        private readonly IProductService _productService;

        public UserCabinetController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var userId = GetUserId();
            var products = _productService.GetProductsByUserId(userId);
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            product.UserId = GetUserId();
            product.CreatedDate = System.DateTime.Now;

            // Simple validation override for internal fields
            ModelState.Remove("UserId");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null || product.UserId != GetUserId())
            {
                return Unauthorized();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var existingProduct = _productService.GetProductById(product.Id);
            if (existingProduct == null || existingProduct.UserId != GetUserId())
            {
                return Unauthorized();
            }

            // Sync original owner and date
            product.UserId = existingProduct.UserId;
            product.CreatedDate = existingProduct.CreatedDate;

            ModelState.Remove("UserId");
            ModelState.Remove("CreatedDate");

            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null || product.UserId != GetUserId())
            {
                return Unauthorized();
            }
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }
}
