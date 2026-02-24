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
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _webHostEnvironment;

        public UserCabinetController(IProductService productService, Microsoft.AspNetCore.Hosting.IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
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
        public async System.Threading.Tasks.Task<IActionResult> Create(Product product, Microsoft.AspNetCore.Http.IFormFile? ImageFile)
        {
            // Simple validation override for internal fields
            ModelState.Remove("UserId");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                // Standardization using Builder pattern
                var builder = new Marketplace.BusinessLogic.Builders.ProductBuilder();

                var newProduct = builder.Reset(product.IsDigital ?? false)
                                        .SetName(product.Name ?? "Unnamed")
                                        .SetPrice(product.Price ?? 0)
                                        .SetStock(product.Stock ?? 0)
                                        .SetDescription(product.Description ?? "")
                                        .SetBrand(product.Brand ?? "")
                                        .SetCategory(product.Category ?? "")
                                        .SetImageUrl(await SaveImage(ImageFile) ?? product.ImageUrl ?? "")
                                        .SetUserId(GetUserId())
                                        .Build();

                newProduct.CreatedDate = System.DateTime.Now;

                _productService.AddProduct(newProduct);
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
        public async System.Threading.Tasks.Task<IActionResult> Edit(Product product, Microsoft.AspNetCore.Http.IFormFile? ImageFile)
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

        public IActionResult Duplicate(int id)
        {
            var original = _productService.GetProductById(id);
            if (original == null || original.UserId != GetUserId())
            {
                return Unauthorized();
            }

            var clone = original.Clone();
            _productService.AddProduct(clone);
            return RedirectToAction("Index");
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
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
