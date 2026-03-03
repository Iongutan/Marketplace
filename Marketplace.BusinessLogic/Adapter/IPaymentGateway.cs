namespace Marketplace.BusinessLogic.Adapter
{
    /// <summary>
    /// Interfața comună (Target) pe care clienții o utilizează.
    /// Adapter Pattern: adaptoarele vor implementa această interfață.
    /// </summary>
    public interface IPaymentGateway
    {
        string GatewayName { get; }
        bool ProcessPayment(string orderId, decimal amount, string currency);
        bool RefundPayment(string transactionId, decimal amount);
        string GetTransactionStatus(string transactionId);
    }
}
