namespace Marketplace.BusinessLogic.Mediator
{
    /// <summary>
    /// Interfața Mediator pentru paternul Mediator (Lab 7).
    /// Centralizează comunicarea dintre participanți.
    /// </summary>
    public interface IMarketplaceMediator
    {
        void SendMessage(string message, ChatParticipant sender);
        void Register(ChatParticipant participant);
        List<ChatMessage> GetHistory();
    }
}
