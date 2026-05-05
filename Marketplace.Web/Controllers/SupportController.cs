using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.ChainOfResponsibility;

namespace Marketplace.Web.Controllers
{
    /// <summary>
    /// Controller pentru paternul Chain of Responsibility (Lab 7).
    /// Cererile de suport sunt procesate printr-un lanț de handlers.
    /// </summary>
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(string customerName, string subject, string description, string type)
        {
            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(subject))
            {
                TempData["Error"] = "Completați toate câmpurile obligatorii.";
                return RedirectToAction("Index");
            }

            var supportType = type switch
            {
                "Delivery" => SupportType.Delivery,
                "Return"   => SupportType.Return,
                _          => SupportType.Store
            };

            var request = new SupportRequest(customerName, subject, description ?? "", supportType);

            // ==== CHAIN OF RESPONSIBILITY ====
            // Construim lanțul: Livrare → Retur → Magazin
            var deliveryHandler = new DeliverySupportHandler();
            var returnHandler = new ReturnSupportHandler();
            var storeHandler = new StoreSupportHandler();

            deliveryHandler.SetNext(returnHandler).SetNext(storeHandler);

            // Procesăm cererea
            var result = deliveryHandler.Handle(request);

            return View("Result", result);
        }
    }
}
