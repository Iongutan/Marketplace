using System;

namespace Marketplace.BusinessLogic.Decorator
{
    /// <summary>
    /// DECORATOR PATTERN — Componenta concretă de bază.
    /// Implementarea minimă a serviciului de notificări:
    /// loghează mesajul în consolă (sau în log-uri reale).
    /// Decoratorii vor "înfășura" această clasă adăugând canale noi.
    /// </summary>
    public class BaseNotificationService : INotificationService
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"[NOTIFICARE DE BAZĂ] Către: {recipient} | Mesaj: {message}");
        }
    }
}
