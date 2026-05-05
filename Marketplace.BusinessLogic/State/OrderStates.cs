namespace Marketplace.BusinessLogic.State
{
    /// <summary>
    /// Starea "Plasată" a comenzii — starea inițială.
    /// </summary>
    public class PlacedState : IOrderState
    {
        public string StateName => "Plasată";
        public string StateDescription => "Comanda a fost plasată și așteaptă confirmarea vânzătorului.";
        public string StateColor => "warning";

        public void Confirm(OrderContext context)
        {
            context.SetState(new ProcessingState());
        }

        public void Ship(OrderContext context)
        {
            throw new InvalidOperationException("Comanda trebuie mai întâi confirmată înainte de expediere.");
        }

        public void Deliver(OrderContext context)
        {
            throw new InvalidOperationException("Comanda nu poate fi livrată fără confirmare și expediere.");
        }

        public void Cancel(OrderContext context)
        {
            context.SetState(new CancelledState());
        }
    }

    /// <summary>
    /// Starea "În Procesare" a comenzii.
    /// </summary>
    public class ProcessingState : IOrderState
    {
        public string StateName => "În Procesare";
        public string StateDescription => "Comanda dvs. este pregătită pentru expediere.";
        public string StateColor => "info";

        public void Confirm(OrderContext context)
        {
            throw new InvalidOperationException("Comanda este deja în procesare.");
        }

        public void Ship(OrderContext context)
        {
            context.SetState(new ShippedState());
        }

        public void Deliver(OrderContext context)
        {
            throw new InvalidOperationException("Comanda trebuie expediată înainte de livrare.");
        }

        public void Cancel(OrderContext context)
        {
            context.SetState(new CancelledState());
        }
    }

    /// <summary>
    /// Starea "Expediată" a comenzii.
    /// </summary>
    public class ShippedState : IOrderState
    {
        public string StateName => "Expediată";
        public string StateDescription => "Comanda este în tranzit și va ajunge în curând la destinatar.";
        public string StateColor => "primary";

        public void Confirm(OrderContext context)
        {
            throw new InvalidOperationException("Comanda este deja expediată.");
        }

        public void Ship(OrderContext context)
        {
            throw new InvalidOperationException("Comanda este deja expediată.");
        }

        public void Deliver(OrderContext context)
        {
            context.SetState(new DeliveredState());
        }

        public void Cancel(OrderContext context)
        {
            throw new InvalidOperationException("Nu se poate anula o comandă deja expediată.");
        }
    }

    /// <summary>
    /// Starea "Livrată" — starea finală de succes.
    /// </summary>
    public class DeliveredState : IOrderState
    {
        public string StateName => "Livrată";
        public string StateDescription => "Comanda a fost livrată cu succes. Vă mulțumim!";
        public string StateColor => "success";

        public void Confirm(OrderContext context) => throw new InvalidOperationException("Comanda este deja finalizată.");
        public void Ship(OrderContext context) => throw new InvalidOperationException("Comanda este deja finalizată.");
        public void Deliver(OrderContext context) => throw new InvalidOperationException("Comanda este deja livrată.");
        public void Cancel(OrderContext context) => throw new InvalidOperationException("Nu se poate anula o comandă deja livrată.");
    }

    /// <summary>
    /// Starea "Anulată" — starea finală de eșec.
    /// </summary>
    public class CancelledState : IOrderState
    {
        public string StateName => "Anulată";
        public string StateDescription => "Comanda a fost anulată.";
        public string StateColor => "danger";

        public void Confirm(OrderContext context) => throw new InvalidOperationException("Comanda este anulată și nu poate fi reactivată.");
        public void Ship(OrderContext context) => throw new InvalidOperationException("Comanda este anulată.");
        public void Deliver(OrderContext context) => throw new InvalidOperationException("Comanda este anulată.");
        public void Cancel(OrderContext context) => throw new InvalidOperationException("Comanda este deja anulată.");
    }
}
