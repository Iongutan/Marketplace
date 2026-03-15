using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Core;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserApi _userApi;
        private readonly IProductService _productService;

        public AccountController(UserApi userApi, IProductService productService)
        {
            _userApi = userApi;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter username and password";
                return View();
            }

            var user = _userApi.ValidateUser(username, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username ?? "Unknown"),
                    new Claim(ClaimTypes.Role, user.Role?.ToString() ?? "Client")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // Remove properties that are not handled by the form to avoid validation issues
            ModelState.Remove("CreatedDate");
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                try
                {
                    // Default role to Client if not specified (handle null or default)
                    if (user.Role == null || (int)user.Role == 0)
                        user.Role = Domain.Enums.UserRole.Client;

                    _userApi.Register(user);
                    return RedirectToAction("Login");
                }
                catch (System.Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            else
            {
                ViewBag.Error = "Please correct the errors in the form.";
            }
            return View(user);
        }

        public IActionResult PublicProfile(int id)
        {
            var user = _userApi.GetUserById(id);
            if (user == null) return NotFound();

            var products = _productService.GetProductsByUserId(id);
            ViewBag.Products = products;

            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
