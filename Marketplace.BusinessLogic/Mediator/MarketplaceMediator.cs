using System;
using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Mediator
{
    public class ChatMessage
    {
        public string SenderName { get; set; }
        public string SenderRole { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;

        public ChatMessage(string senderName, string senderRole, string content)
        {
            SenderName = senderName;
            SenderRole = senderRole;
            Content = content;
        }
    }

    /// <summary>
    /// Clasa abstractă pentru participanții la chat — paternul Mediator (Lab 7).
    /// Participanții nu comunică direct, ci prin mediator.
    /// </summary>
    public abstract class ChatParticipant
    {
        protected IMarketplaceMediator _mediator;
        public string Name { get; }
        public abstract string Role { get; }

        protected ChatParticipant(string name, IMarketplaceMediator mediator)
        {
            Name = name;
            _mediator = mediator;
            _mediator.Register(this);
        }

        public void Send(string message)
        {
            _mediator.SendMessage(message, this);
        }

        public virtual void Receive(string message, string fromName, string fromRole)
        {
            // Participanții primesc mesajul prin mediator
        }
    }

    /// <summary>
    /// Mediatorul centralizat — gestionează toate mesajele din platformă.
    /// </summary>
    public class MarketplaceMediator : IMarketplaceMediator
    {
        private readonly List<ChatParticipant> _participants = new();
        private readonly List<ChatMessage> _history = new();

        public void Register(ChatParticipant participant)
        {
            if (!_participants.Contains(participant))
                _participants.Add(participant);
        }

        public void SendMessage(string message, ChatParticipant sender)
        {
            var chatMsg = new ChatMessage(sender.Name, sender.Role, message);
            _history.Add(chatMsg);

            // Transmite mesajul tuturor participanților, exceptând expeditorul
            foreach (var participant in _participants)
            {
                if (participant != sender)
                    participant.Receive(message, sender.Name, sender.Role);
            }
        }

        public List<ChatMessage> GetHistory() => _history;
    }

    public class BuyerParticipant : ChatParticipant
    {
        public override string Role => "Cumpărător";
        public BuyerParticipant(string name, IMarketplaceMediator mediator) : base(name, mediator) { }
    }

    public class SellerParticipant : ChatParticipant
    {
        public override string Role => "Vânzător";
        public SellerParticipant(string name, IMarketplaceMediator mediator) : base(name, mediator) { }
    }

    public class SupportParticipant : ChatParticipant
    {
        public override string Role => "Suport";
        public SupportParticipant(string name, IMarketplaceMediator mediator) : base(name, mediator) { }
    }
}
