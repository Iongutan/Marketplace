using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.Domain.Entities;
using System.Linq;

namespace Marketplace.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _webHostEnvironment;

        public AdminController(IProductService productService, Microsoft.AspNetCore.Hosting.IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Dashboard()
        {
            var products = _productService.GetProducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> EditProduct(Product product, Microsoft.AspNetCore.Http.IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                    var uniqueFileName = System.Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    product.ImageUrl = "/uploads/products/" + uniqueFileName;
                }
                _productService.UpdateProduct(product);
                return RedirectToAction("Dashboard");
            }
            return View(product);
        }

        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Dashboard");
        }

        public IActionResult DuplicateProduct(int id)
        {
            var original = _productService.GetProductById(id);
            if (original == null) return NotFound();

            var clone = original.Clone();
            clone.Id = 0;
            clone.Name = "Copie a " + original.Name;
            clone.CreatedDate = System.DateTime.Now;

            _productService.AddProduct(clone);
            return RedirectToAction("Dashboard");
        }

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

            // Ensure Admin details
            product.CreatedDate = System.DateTime.Now;

            _productService.AddProduct(product);
            return RedirectToAction("Dashboard");
        }
    }
}
