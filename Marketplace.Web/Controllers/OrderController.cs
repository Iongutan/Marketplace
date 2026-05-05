using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Facade;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.BusinessLogic.State;

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
        [HttpGet]
        public IActionResult Checkout(int productId, string strategyType = "Normal")
        {
            var product = _productService.GetProductById(productId);
            if (product == null) return NotFound();

            // ==== 2. STRATEGY PATTERN LOCAT AICI ====
            decimal basePrice = product.Price ?? 0m;
            Marketplace.BusinessLogic.Strategy.IPricingStrategy strategy = strategyType switch
            {
                "Holiday" => new Marketplace.BusinessLogic.Strategy.HolidayDiscountStrategy(),
                "Clearance" => new Marketplace.BusinessLogic.Strategy.ClearanceStrategy(),
                _ => new Marketplace.BusinessLogic.Strategy.RegularPricingStrategy()
            };

            var calc = new Marketplace.BusinessLogic.Strategy.PriceCalculatorContext(strategy);
            var finalPrice = calc.Calculate(basePrice);

            ViewBag.Product = product;
            ViewBag.FinalPrice = finalPrice;
            ViewBag.StrategyType = strategyType;

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
                pricePerUnit: product.Price ?? 0m, // Pentru demo considerăm prețul intreg la finalizare comanda intern, dar in viitor trimitem FinalPrice

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

        // ==== STATE PATTERN (Lab 7) ====
        /// <summary>
        /// Pagina de tracking a comenzii cu mașina de stări.
        /// Demo interactiv: Admin poate avansa starea comenzii.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult OrderStatus(int productId = 0, string currentState = "Placed")
        {
            // Creăm un OrderContext demo bazat pe produsul selectat
            string productName = "Produs Demo";
            decimal price = 0m;

            if (productId > 0)
            {
                var product = _productService.GetProductById(productId);
                if (product != null)
                {
                    productName = product.Name ?? "Produs";
                    price = product.Price ?? 0m;
                }
            }

            var order = new OrderContext(
                orderId: productId > 0 ? productId * 100 : 1001,
                productName: productName,
                totalPrice: price,
                customerName: User.Identity?.Name ?? "Client"
            );

            // Simulăm starea curentă cerută
            try
            {
                if (currentState == "Processing" || currentState == "Shipped" || currentState == "Delivered")
                    order.Confirm();
                if (currentState == "Shipped" || currentState == "Delivered")
                    order.Ship();
                if (currentState == "Delivered")
                    order.Deliver();
                if (currentState == "Cancelled")
                    order.Cancel();
            }
            catch { /* ignorăm erorile de tranziție în demo */ }

            ViewBag.OrderContext = order;
            ViewBag.ProductId = productId;
            return View();
        }
    }
}
