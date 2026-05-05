using Microsoft.AspNetCore.Mvc;
using Marketplace.BusinessLogic.Command;
using Marketplace.BusinessLogic.Memento;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.BusinessLogic.Singletons;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var cartItems = new List<Marketplace.Domain.Entities.Product>();
            foreach(var strId in CartManager.Instance.Cart.Items)
            {
                if(int.TryParse(strId, out int id))
                {
                    var prod = _productService.GetProductById(id);
                    if (prod != null) cartItems.Add(prod);
                }
            }
            return View(cartItems);
        }

        public IActionResult AddToCart(string productId)
        {
            if(!string.IsNullOrEmpty(productId))
            {
                var mgr = CartManager.Instance;
                // Command Pattern -> Execute
                var cmd = new AddToCartCommand(mgr.Cart, productId);
                mgr.Invoker.ExecuteCommand(cmd);
                
                // Memento Pattern -> Sync state
                mgr.Originator.RestoreState(new CartState(mgr.Cart.Items));
            }
            return Redirect(Request.Headers.Referer.ToString() is { Length: > 0 } referer ? referer : Url.Action("Index", "Home")!);
        }

        public IActionResult UndoLastAdd()
        {
            var mgr = CartManager.Instance;
            // Command Pattern -> Undo
            mgr.Invoker.UndoLastCommand();
            
            // Sync Memento
            mgr.Originator.RestoreState(new CartState(mgr.Cart.Items));
            
            return RedirectToAction("Index");
        }

        public IActionResult SaveCart()
        {
            var mgr = CartManager.Instance;
            // Memento Pattern -> Backup (Save State)
            mgr.Caretaker.Backup();
            TempData["Message"] = "Starea curentă a coșului a fost salvată cu succes.";
            return RedirectToAction("Index");
        }

        public IActionResult RestoreCart()
        {
            var mgr = CartManager.Instance;
            // Memento Pattern -> Restore
            mgr.Caretaker.Undo();
            
            // Sync Command Context
            mgr.Cart.Items.Clear();
            foreach(var item in mgr.Originator.Items) {
                mgr.Cart.AddItem(item);
            }
            
            TempData["Message"] = "Coșul a fost restaurat la versiunea salvată anterioară.";
            return RedirectToAction("Index");
        }
    }
}
