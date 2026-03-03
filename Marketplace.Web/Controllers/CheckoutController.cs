using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Adapter;
using Marketplace.BusinessLogic.Interfaces;

namespace Marketplace.Web.Controllers
{
    /// <summary>
    /// Controller demonstrând Adapter Pattern:
    /// PayPal, Stripe și Google Pay sunt adaptate la interfața comună IPaymentGateway.
    /// </summary>
    public class CheckoutController : Controller
    {
        private readonly IProductService _productService;

        public CheckoutController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        public IActionResult Index(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null) return NotFound();

            ViewBag.ProductId = productId;
            ViewBag.ProductName = product.Name;
            ViewBag.ProductPrice = product.Price ?? 0m;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ProcessPayment(int productId, string gateway, string cardNumber = "4111111111111111")
        {
            var product = _productService.GetProductById(productId);
            if (product == null) return NotFound();

            // ── Adapter Pattern: selectăm adaptorul potrivit în funcție de gateway ──
            IPaymentGateway paymentGateway = gateway switch
            {
                "Stripe" => new StripeAdapter(),
                "GooglePay" => new GooglePayAdapter(),
                _ => new PayPalAdapter()          // implicit PayPal
            };

            decimal amount = product.Price ?? 0m;
            bool success = paymentGateway.ProcessPayment($"ORDER-{productId}", amount, "MDL");

            ViewBag.GatewayName = paymentGateway.GatewayName;
            ViewBag.ProductName = product.Name;
            ViewBag.Amount = amount;
            ViewBag.Success = success;
            ViewBag.GatewayType = gateway;

            return View("PaymentResult");
        }
    }
}
