using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.BusinessLogic.Core;
using Marketplace.Domain.Entities;
using Marketplace.BusinessLogic.TemplateMethod;
using Marketplace.BusinessLogic.Visitor;
using System.Linq;
using System.Text;

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
        public async System.Threading.Tasks.Task<IActionResult> EditProduct(Product product, Microsoft.AspNetCore.Http.IFormFile? ImageFile, string? pricingStrategy)
        {
            // Remove fields not needed for re-validation on edit
            ModelState.Remove("UserId");
            ModelState.Remove("CreatedDate");

            if (ModelState.IsValid)
            {
                var oldProduct = _productService.GetProductById(product.Id);
                decimal oldPrice = oldProduct?.Price ?? 0m;

                // ==== STRATEGY PATTERN ====
                // Apply the chosen pricing strategy to the base price
                Marketplace.BusinessLogic.Strategy.IPricingStrategy strategy = pricingStrategy switch
                {
                    "holiday"   => new Marketplace.BusinessLogic.Strategy.HolidayDiscountStrategy(),
                    "clearance" => new Marketplace.BusinessLogic.Strategy.ClearanceStrategy(),
                    _           => new Marketplace.BusinessLogic.Strategy.RegularPricingStrategy()
                };
                var calculator = new Marketplace.BusinessLogic.Strategy.PriceCalculatorContext(strategy);
                decimal basePrice = product.Price ?? 0m;
                // Round to whole number — no .99 decimals
                product.Price = Math.Round(calculator.Calculate(basePrice), 0, MidpointRounding.AwayFromZero);

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

                // ==== OBSERVER PATTERN ====
                if (oldProduct != null && product.Price != oldPrice)
                {
                    var subject = new Marketplace.BusinessLogic.Observer.ProductPriceSubject(product.Name ?? "Produs", oldPrice);
                    subject.Attach(new Marketplace.BusinessLogic.Observer.CustomerObserver("Abonați Newsletter"));
                    subject.Attach(new Marketplace.BusinessLogic.Observer.CustomerObserver("Utilizatorul VIP"));

                    subject.Price = product.Price ?? 0m; // This triggers Notify() internally

                    string strategyLabel = pricingStrategy switch
                    {
                        "holiday"   => "Reducere Sărbători (−20%)",
                        "clearance" => "Lichidare Stoc (−50%)",
                        _           => "Preț Regulat"
                    };
                    TempData["ObserverMessage"] = $"Abonații au fost notificați: prețul la \"{product.Name}\" s-a schimbat de la {(int)oldPrice} MDL la {(int)(product.Price ?? 0)} MDL. Strategie aplicată: {strategyLabel}.";
                }

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

        // ==== TEMPLATE METHOD PATTERN (Lab 7) ====
        [HttpGet]
        public IActionResult Reports(string reportType = "Sales")
        {
            var products = _productService.GetProducts().ToList();

            ReportGenerator generator = reportType switch
            {
                "UserActivity" => new UserActivityReport(),
                "Inventory"   => new InventoryReport(),
                _             => new ProductSalesReport()
            };

            ViewBag.ReportHtml = generator.GenerateReport(products);
            ViewBag.ReportType = reportType;
            ViewBag.ReportTitle = generator.ReportTitle;
            return View();
        }

        // ==== VISITOR PATTERN (Lab 7) ====
        [HttpGet]
        public IActionResult ExportProducts(string format = "Html")
        {
            var products = _productService.GetProducts().ToList();

            IProductVisitor visitor = format switch
            {
                "Csv"  => new CsvExportVisitor(),
                "Json" => new JsonExportVisitor(),
                _      => new HtmlExportVisitor()
            };

            // Aplicăm vizitorul pe fiecare produs
            foreach (var product in products)
            {
                var visitable = new VisitableProduct(product);
                visitable.Accept(visitor);
            }

            string result = visitor.GetResult();

            if (format == "Csv")
                return File(Encoding.UTF8.GetBytes(result), "text/csv", "produse_export.csv");
            if (format == "Json")
                return File(Encoding.UTF8.GetBytes(result), "application/json", "produse_export.json");

            ViewBag.ExportHtml = result;
            ViewBag.Format = format;
            return View();
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
