using Marketplace.BusinessLogic.Command;
using Marketplace.BusinessLogic.Memento;
using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Singletons
{
    public class CartManager
    {
        private static readonly CartManager _instance = new CartManager();
        
        public CartContext Cart { get; } = new CartContext();
        public CartInvoker Invoker { get; } = new CartInvoker();
        public ShoppingCartOriginator Originator { get; } = new ShoppingCartOriginator();
        public CartHistoryCaretaker Caretaker { get; }

        private CartManager()
        {
            Caretaker = new CartHistoryCaretaker(Originator);
        }

        public static CartManager Instance => _instance;
        
        public void Clear()
        {
            Cart.Items.Clear();
            Originator.Items.Clear();
            // Could also clear care taker if needed
        }
    }
}
