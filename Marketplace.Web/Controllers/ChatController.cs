using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Mediator;
using System.Collections.Generic;

namespace Marketplace.Web.Controllers
{
    /// <summary>
    /// Controller pentru paternul Mediator (Lab 7).
    /// Participanții (cumpărător, vânzător, suport) comunică exclusiv prin mediator.
    /// </summary>
    public class ChatController : Controller
    {
        // Mediator static în memorie pentru demo (în producție ar fi în sesiune/DB)
        private static readonly MarketplaceMediator _mediator = new();
        private static BuyerParticipant?   _buyer;
        private static SellerParticipant?  _seller;
        private static SupportParticipant? _support;

        static ChatController()
        {
            // Inițializăm participanții și îi înregistrăm automat la mediator
            _buyer   = new BuyerParticipant("Ion Cumpărătorul", _mediator);
            _seller  = new SellerParticipant("Maria Vânzătoarea", _mediator);
            _support = new SupportParticipant("Suport Marketplace", _mediator);

            // Mesaje demo inițiale
            _buyer.Send("Bună ziua! Aș dori să știu dacă produsul este disponibil.");
            _seller.Send("Bună! Da, produsul este în stoc și poate fi livrat în 2 zile.");
            _support.Send("Vă informăm că toate tranzacțiile sunt protejate de platforma noastră.");
        }

        public IActionResult Index()
        {
            ViewBag.History = _mediator.GetHistory();
            return View();
        }

        [HttpPost]
        public IActionResult Send(string senderRole, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                switch (senderRole)
                {
                    case "Buyer":   _buyer?.Send(message);   break;
                    case "Seller":  _seller?.Send(message);  break;
                    case "Support": _support?.Send(message); break;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reset()
        {
            // Reinițializare mediator
            var newMediator = new MarketplaceMediator();
            _buyer   = new BuyerParticipant("Ion Cumpărătorul", newMediator);
            _seller  = new SellerParticipant("Maria Vânzătoarea", newMediator);
            _support = new SupportParticipant("Suport Marketplace", newMediator);

            // Copiaza referința mediatorului nou în câmpul static prin truc
            typeof(ChatController)
                .GetField("_mediator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, newMediator);

            return RedirectToAction("Index");
        }
    }
}
