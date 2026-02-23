using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.Domain.Entities;
using System.Linq;

namespace Marketplace.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductService productService, Microsoft.AspNetCore.Hosting.IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string? category, string? brand, string? search)
        {
            var products = _productService.GetProducts();

            if (!string.IsNullOrEmpty(category))
            {
                if (category == "Produse Online")
                {
                    products = products.Where(p => p.Category == "Produse Online" || p.Category == "Software" || p.IsDigital == true);
                }
                else if (category == "Produse Electronice")
                {
                    products = products.Where(p => p.Category == "Produse Electronice" || p.Category == "Electronice" || p.Category == "Electronics");
                }
                else if (category == "Interior")
                {
                    products = products.Where(p => p.Category == "Interior" || p.Category == "Mobilă" || p.Category == "Produse Fizice");
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

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Create(Product product, bool? isDigital, Microsoft.AspNetCore.Http.IFormFile? ImageFile)
        {
            // Simple validation override
            ModelState.Remove("UserId");
            ModelState.Remove("CreatedDate");

            if (ModelState.IsValid)
            {
                // Use Factory Method
                Marketplace.BusinessLogic.Factories.ProductFactory factory;
                bool digitalValue = isDigital ?? product.IsDigital ?? false;

                if (digitalValue)
                    factory = new Marketplace.BusinessLogic.Factories.DigitalProductFactory();
                else
                    factory = new Marketplace.BusinessLogic.Factories.PhysicalProductFactory();

                var newProduct = factory.CreateProduct(product.Name ?? "Unnamed", product.Price ?? 0, product.Stock ?? 0);
                newProduct.Description = product.Description;
                newProduct.Brand = product.Brand;
                newProduct.Category = product.Category;
                newProduct.ImageUrl = await SaveImage(ImageFile) ?? product.ImageUrl;
                newProduct.IsDigital = digitalValue;

                // Get current user ID
                var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                newProduct.UserId = claim != null ? int.Parse(claim.Value) : 0;
                newProduct.CreatedDate = System.DateTime.Now;

                _productService.AddProduct(newProduct);
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
        public async System.Threading.Tasks.Task<IActionResult> Edit(Product product, Microsoft.AspNetCore.Http.IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var uploadedPath = await SaveImage(ImageFile);
                if (uploadedPath != null)
                {
                    product.ImageUrl = uploadedPath;
                }
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
        [Authorize]
        public IActionResult Duplicate(int id)
        {
            var original = _productService.GetProductById(id);
            if (original == null) return NotFound();

            var clone = original.Clone();
            clone.Id = 0; // Reset Id for EF to treat as new entity
            clone.Name = "Copie a " + original.Name;

            _productService.AddProduct(clone);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult QuickCreate(string type)
        {
            var builder = new Marketplace.BusinessLogic.Builders.ProductBuilder();
            var director = new Marketplace.BusinessLogic.Builders.ProductDirector();
            Product product;

            if (type == "Laptop")
                product = director.ConstructLaptop(builder);
            else if (type == "EBook")
                product = director.ConstructEBook(builder);
            else
                return BadRequest("Unknown product type");

            _productService.AddProduct(product);
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Checkout(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            // Use Abstract Factory to determine shipping and payment
            Marketplace.BusinessLogic.AbstractFactory.IOrderFactory orderFactory;
            if (product.IsDigital == true)
                orderFactory = new Marketplace.BusinessLogic.AbstractFactory.DigitalOrderFactory();
            else
                orderFactory = new Marketplace.BusinessLogic.AbstractFactory.PhysicalOrderFactory();

            var shipping = orderFactory.CreateShippingMethod();
            var payment = orderFactory.CreatePaymentMethod();

            // In a real app, these would actually process things. Here we just show it works.
            ViewBag.Message = $"Comandă procesată pentru {product.Name}. " +
                             $"Metodă livrare: {shipping.GetType().Name}, " +
                             $"Metodă plată: {payment.GetType().Name}.";

            return View("OrderResult");
        }
        private async System.Threading.Tasks.Task<string?> SaveImage(Microsoft.AspNetCore.Http.IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                var uniqueFileName = System.Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return "/uploads/products/" + uniqueFileName;
            }
            return null;
        }
    }
}
