using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Memento
{
    public class CartState
    {
        public List<string> Items { get; }

        public CartState(List<string> items)
        {
            // Salvează o copie izolată a stării coșului
            Items = new List<string>(items);
        }
    }

    public class ShoppingCartOriginator
    {
        public List<string> Items { get; private set; } = new List<string>();

        public void AddItem(string item)
        {
            Items.Add(item);
        }

        public CartState SaveState()
        {
            return new CartState(Items);
        }

        public void RestoreState(CartState state)
        {
            Items = new List<string>(state.Items);
        }
    }

    public class CartHistoryCaretaker
    {
        private readonly Stack<CartState> _history = new Stack<CartState>();
        private readonly ShoppingCartOriginator _originator;

        public CartHistoryCaretaker(ShoppingCartOriginator originator)
        {
            _originator = originator;
        }

        public void Backup()
        {
            _history.Push(_originator.SaveState());
        }

        public void Undo()
        {
            if (_history.Count == 0) return;

            var prevState = _history.Pop();
            _originator.RestoreState(prevState);
        }
    }
}
