using System;

namespace Marketplace.BusinessLogic.Decorator
{
    /// <summary>
    /// DECORATOR PATTERN — Decorator SMS.
    /// Adaugă trimiterea unui SMS pe lângă notificările precedente.
    /// Poate fi înlănțuit cu EmailNotificationDecorator sau orice alt decorator.
    /// </summary>
    public class SmsNotificationDecorator : NotificationDecorator
    {
        private readonly string _senderNumber;

        public SmsNotificationDecorator(INotificationService wrapped,
                                         string senderNumber = "+37360000000")
            : base(wrapped)
        {
            _senderNumber = senderNumber;
        }

        public override void Send(string recipient, string message)
        {
            // 1. Apelăm lanțul de decoratori precedent
            base.Send(recipient, message);

            // 2. Adăugăm comportamentul nou: SMS (trunchiem la 160 caractere)
            string smsText = message.Length > 160 ? message[..157] + "..." : message;
            Console.WriteLine($"[📱 SMS] De la: {_senderNumber} → Către: {recipient}");
            Console.WriteLine($"    SMS ({smsText.Length} chars): {smsText}");
        }
    }
}
