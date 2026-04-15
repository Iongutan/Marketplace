using System;

namespace Marketplace.BusinessLogic.Decorator
{
    /// <summary>
    /// DECORATOR PATTERN — Decorator Email.
    /// Adaugă trimiterea unui email pe lângă notificarea de bază.
    /// Înfășoară orice INotificationService existent.
    /// </summary>
    public class EmailNotificationDecorator : NotificationDecorator
    {
        private readonly string _smtpFrom;

        public EmailNotificationDecorator(INotificationService wrapped,
                                          string smtpFrom = "noreply@marketplace.md")
            : base(wrapped)
        {
            _smtpFrom = smtpFrom;
        }

        public override void Send(string recipient, string message)
        {
            // 1. Apelăm componenta înfășurată (baza sau alt decorator)
            base.Send(recipient, message);

            // 2. Adăugăm comportamentul nou: Email
            Console.WriteLine($"[📧 EMAIL] De la: {_smtpFrom} → Către: {recipient}");
            Console.WriteLine($"    Subiect: Notificare Marketplace | Conținut: {message}");
        }
    }
}
