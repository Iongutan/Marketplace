using System;

namespace Marketplace.BusinessLogic.Decorator
{
    /// <summary>
    /// DECORATOR PATTERN — Decoratorul abstract de bază.
    /// Înfășoară un INotificationService și îl apelează prin delegare.
    /// Subclasele (EmailNotificationDecorator etc.) extind comportamentul
    /// adăugând propriile canale FĂRĂ a modifica clasa de bază.
    /// </summary>
    public abstract class NotificationDecorator : INotificationService
    {
        protected readonly INotificationService _wrapped;

        protected NotificationDecorator(INotificationService wrapped)
        {
            _wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }

        /// <summary>
        /// Delegare implicită la componenta înfășurată.
        /// Subclasele suprascriu această metodă adăugând propriul comportament.
        /// </summary>
        public virtual void Send(string recipient, string message)
        {
            _wrapped.Send(recipient, message);
        }
    }
}
