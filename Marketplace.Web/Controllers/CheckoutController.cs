using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Adapter;
using Marketplace.BusinessLogic.Interfaces;

namespace Marketplace.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IProductService _productService;

        public CheckoutController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index(int? productId, string strategyType = "Normal")
        {
            decimal basePrice = 0m;
            string itemsName = "";
            bool isCart = false;

            if (productId.HasValue && productId.Value > 0)
            {
                var product = _productService.GetProductById(productId.Value);
                if (product == null) return NotFound();
                basePrice = product.Price ?? 0m;
                itemsName = product.Name ?? "Produs";
                ViewBag.ProductId = productId.Value;
            }
            else
            {
                var mgr = Marketplace.BusinessLogic.Singletons.CartManager.Instance;
                if(mgr.Cart.Items.Count == 0) return RedirectToAction("Index", "Cart");
                itemsName = $"Coș de cumpărături ({mgr.Cart.Items.Count} articole)";
                foreach(var idStr in mgr.Cart.Items)
                {
                    if(int.TryParse(idStr, out int id)) {
                        var p = _productService.GetProductById(id);
                        if (p != null) basePrice += p.Price ?? 0m;
                    }
                }
                isCart = true;
            }

            // ==== STRATEGY PATTERN ====
            Marketplace.BusinessLogic.Strategy.IPricingStrategy strategy = strategyType switch
            {
                "Holiday" => new Marketplace.BusinessLogic.Strategy.HolidayDiscountStrategy(),
                "Clearance" => new Marketplace.BusinessLogic.Strategy.ClearanceStrategy(),
                _ => new Marketplace.BusinessLogic.Strategy.RegularPricingStrategy()
            };

            var calc = new Marketplace.BusinessLogic.Strategy.PriceCalculatorContext(strategy);
            var finalPrice = calc.Calculate(basePrice);

            ViewBag.ProductName = itemsName;
            ViewBag.ProductPrice = finalPrice;
            ViewBag.StrategyType = strategyType;
            ViewBag.IsCart = isCart;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ProcessPayment(int? productId, string gateway, decimal amountPaid, string cardNumber = "4111111111111111")
        {
            string itemsName = "Plată Coș";
            if (productId.HasValue && productId.Value > 0)
            {
                var product = _productService.GetProductById(productId.Value);
                if (product != null) itemsName = product.Name ?? "Produs";
            }
            else
            {
                // Clear cart if successful
                Marketplace.BusinessLogic.Singletons.CartManager.Instance.Clear();
            }

            IPaymentGateway paymentGateway = gateway switch
            {
                "Stripe" => new StripeAdapter(),
                "GooglePay" => new GooglePayAdapter(),
                _ => new PayPalAdapter()          // implicit PayPal
            };

            bool success = paymentGateway.ProcessPayment(productId.HasValue ? $"ORDER-{productId.Value}" : $"CART-{Guid.NewGuid()}", amountPaid, "MDL");

            ViewBag.GatewayName = paymentGateway.GatewayName;
            ViewBag.ProductName = itemsName;
            ViewBag.Amount = amountPaid;
            ViewBag.Success = success;
            ViewBag.GatewayType = gateway;

            return View("PaymentResult");
        }
    }
}
