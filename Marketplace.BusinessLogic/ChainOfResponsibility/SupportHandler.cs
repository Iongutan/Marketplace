namespace Marketplace.BusinessLogic.ChainOfResponsibility
{
    /// <summary>
    /// Handler abstract pentru paternul Chain of Responsibility (Lab 7).
    /// Fiecare handler poate procesa cererea sau o poate transmite mai departe.
    /// </summary>
    public abstract class SupportHandler
    {
        protected SupportHandler? _nextHandler;

        public SupportHandler SetNext(SupportHandler nextHandler)
        {
            _nextHandler = nextHandler;
            return nextHandler;
        }

        public virtual SupportRequest Handle(SupportRequest request)
        {
            if (_nextHandler != null)
                return _nextHandler.Handle(request);
            
            // Dacă nimeni nu poate gestiona, marcăm ca nerezolvat
            request.HandledBy = "Sistem Automat";
            request.Resolution = "Cererea dvs. a fost înregistrată și va fi procesată în 48 de ore.";
            request.IsResolved = true;
            return request;
        }
    }
}
