namespace Marketplace.BusinessLogic.State
{
    /// <summary>
    /// Interfața State pentru paternul State (Lab 7).
    /// Fiecare stare a comenzii definește comportamentul său propriu.
    /// </summary>
    public interface IOrderState
    {
        string StateName { get; }
        string StateDescription { get; }
        string StateColor { get; } // bootstrap badge color
        void Confirm(OrderContext context);
        void Ship(OrderContext context);
        void Deliver(OrderContext context);
        void Cancel(OrderContext context);
    }
}
