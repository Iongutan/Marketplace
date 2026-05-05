using System;
using System.Collections.Generic;

namespace Marketplace.BusinessLogic.State
{
    /// <summary>
    /// Contextul paternului State — menține starea curentă a unei comenzi.
    /// Starea se poate schimba prin tranziții definite de fiecare stare în parte.
    /// </summary>
    public class OrderContext
    {
        private IOrderState _currentState;
        public int OrderId { get; }
        public string ProductName { get; }
        public decimal TotalPrice { get; }
        public string CustomerName { get; }
        private readonly List<string> _history = new();

        public OrderContext(int orderId, string productName, decimal totalPrice, string customerName)
        {
            OrderId = orderId;
            ProductName = productName;
            TotalPrice = totalPrice;
            CustomerName = customerName;
            _currentState = new PlacedState();
            _history.Add($"[{DateTime.Now:HH:mm}] Comanda plasată — stare inițială: {_currentState.StateName}");
        }

        public void SetState(IOrderState state)
        {
            _currentState = state;
            _history.Add($"[{DateTime.Now:HH:mm}] Tranziție la starea: {state.StateName}");
        }

        public IOrderState CurrentState => _currentState;
        public IReadOnlyList<string> History => _history;

        // Acțiuni disponibile — delegate la starea curentă
        public void Confirm() => _currentState.Confirm(this);
        public void Ship()    => _currentState.Ship(this);
        public void Deliver() => _currentState.Deliver(this);
        public void Cancel()  => _currentState.Cancel(this);

        public bool CanConfirm => _currentState is PlacedState;
        public bool CanShip    => _currentState is ProcessingState;
        public bool CanDeliver => _currentState is ShippedState;
        public bool CanCancel  => _currentState is PlacedState || _currentState is ProcessingState;
    }
}
