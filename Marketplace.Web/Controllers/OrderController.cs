using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Facade;
using Marketplace.BusinessLogic.Interfaces;

namespace Marketplace.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductService _productService;
        private readonly OrderFacade _orderFacade = new OrderFacade();

        public OrderController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        public IActionResult Checkout(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null) return NotFound();

            ViewBag.Product = product;
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PlaceOrder(int productId, string paymentGateway, int quantity = 1)
        {
            var product = _productService.GetProductById(productId);
            if (product == null) return NotFound();

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                         ?? User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value
                         ?? "client@marketplace.md";
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "Client";

            var result = _orderFacade.PlaceOrder(
                productId: product.Id,
                productName: product.Name ?? "Produs",
                pricePerUnit: product.Price ?? 0m,
                isDigital: product.IsDigital ?? false,
                sellerId: product.UserId ?? 1,
                buyerName: userName,
                buyerEmail: userEmail,
                quantity: quantity,
                paymentGateway: paymentGateway ?? "PayPal"
            );

            if (result == null)
            {
                TempData["Error"] = "Comanda nu a putut fi procesată. Verificați disponibilitatea produsului.";
                return RedirectToAction("Checkout", new { productId });
            }

            return View("OrderConfirmation", result);
        }
    }
}
