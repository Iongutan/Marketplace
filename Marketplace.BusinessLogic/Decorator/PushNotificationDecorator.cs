using System;

namespace Marketplace.BusinessLogic.Decorator
{
    /// <summary>
    /// DECORATOR PATTERN — Decorator Push Notification.
    /// Adaugă o notificare push (browser/mobile) pe lângă cele precedente.
    /// Demonstrează că decoratorii pot fi înlănțuiți în orice ordine:
    ///   Base → Email → SMS → Push  (sau orice altă combinație).
    /// </summary>
    public class PushNotificationDecorator : NotificationDecorator
    {
        private readonly string _appId;

        public PushNotificationDecorator(INotificationService wrapped,
                                          string appId = "marketplace-app")
            : base(wrapped)
        {
            _appId = appId;
        }

        public override void Send(string recipient, string message)
        {
            // 1. Apelăm lanțul de decoratori precedent
            base.Send(recipient, message);

            // 2. Adăugăm comportamentul nou: Push Notification
            Console.WriteLine($"[🔔 PUSH] AppId: {_appId} → User: {recipient}");
            Console.WriteLine($"    Notificare push: {message[..Math.Min(50, message.Length)]}...");
        }
    }
}
