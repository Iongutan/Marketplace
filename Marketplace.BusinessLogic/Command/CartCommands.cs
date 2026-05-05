using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Command
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class CartContext
    {
        public List<string> Items { get; private set; } = new List<string>();

        public void AddItem(string item)
        {
            Items.Add(item);
        }

        public void RemoveItem(string item)
        {
            Items.Remove(item);
        }
    }

    public class AddToCartCommand : ICommand
    {
        private readonly CartContext _cart;
        private readonly string _item;

        public AddToCartCommand(CartContext cart, string item)
        {
            _cart = cart;
            _item = item;
        }

        public void Execute()
        {
            _cart.AddItem(_item);
        }

        public void Undo()
        {
            _cart.RemoveItem(_item);
        }
    }

    public class CartInvoker
    {
        private readonly Stack<ICommand> _commandHistory = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commandHistory.Push(command);
        }

        public void UndoLastCommand()
        {
            if (_commandHistory.Count > 0)
            {
                var command = _commandHistory.Pop();
                command.Undo();
            }
        }
    }
}
