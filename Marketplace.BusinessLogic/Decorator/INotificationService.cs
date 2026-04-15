namespace Marketplace.BusinessLogic.Decorator
{
    /// <summary>
    /// DECORATOR PATTERN — Interfața de bază a serviciului de notificări.
    /// Atât implementarea concretă (BaseNotificationService) cât și
    /// decoratorii (Email, SMS, Push) implementează această interfață,
    /// permițând înlănțuirea dinamică a comportamentelor.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Trimite o notificare destinatarului cu mesajul specificat.
        /// Fiecare decorator adaugă propriul canal de notificare
        /// înainte sau după a delega la componenta înfășurată.
        /// </summary>
        void Send(string recipient, string message);
    }
}
