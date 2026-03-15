using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.BusinessLogic.Core;
using Marketplace.Domain.Entities;
using System.Linq;

namespace Marketplace.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserApi _userApi;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _webHostEnvironment;

        public AdminController(IProductService productService, UserApi userApi, Microsoft.AspNetCore.Hosting.IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _userApi = userApi;
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

            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                clone.UserId = int.Parse(claim.Value);
            }

            _productService.AddProduct(clone);
            return RedirectToAction("Dashboard");
        }

        public IActionResult QuickCreate(string type)
        {
            // Prototype Pattern: Try to find an existing product to use as a template
            string category = type == "Laptop" ? "Produse Electronice" : "Produse Online";
            var template = _productService.GetProducts()
                                          .FirstOrDefault(p => p.Category == category);

            Product product;
            if (template != null)
            {
                // Use Prototype
                product = template.CloneAsTemplate();
                // Optionally adjust specific fields
                if (type == "Laptop") product.Name = "Premium Laptop Pro (Template)";
                else product.Name = "Mastering Design Patterns (Template)";
            }
            else
            {
                // Fallback to Builder
                var builder = new Marketplace.BusinessLogic.Builders.ProductBuilder();
                var director = new Marketplace.BusinessLogic.Builders.ProductDirector();

                product = (type switch
                {
                    "Laptop" => director.ConstructLaptop(builder),
                    "EBook" => director.ConstructEBook(builder),
                    "Smartphone" => director.ConstructSmartphone(builder),
                    "Course" => director.ConstructCourse(builder),
                    "Furniture" => director.ConstructFurniture(builder),
                    _ => null
                })!;

                if (product == null) return BadRequest("Unknown product type");
            }

            product.CreatedDate = System.DateTime.Now;

            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                product.UserId = int.Parse(claim.Value);
            }

            _productService.AddProduct(product);
            return RedirectToAction("Dashboard");
        }
        public IActionResult Users()
        {
            var users = _userApi.GetAllUsers();
            return View(users);
        }

        public IActionResult DeleteUser(int id)
        {
            // 1. Delete all products belonging to this user
            var products = _productService.GetProductsByUserId(id);
            foreach (var p in products)
            {
                _productService.DeleteProduct(p.Id);
            }

            // 2. Delete the user
            _userApi.DeleteUser(id);

            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _userApi.GetUserById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user, string? newPassword)
        {
            try
            {
                _userApi.UpdateUser(user, newPassword);
                return RedirectToAction("Users");
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(user);
            }
        }
    }
}
