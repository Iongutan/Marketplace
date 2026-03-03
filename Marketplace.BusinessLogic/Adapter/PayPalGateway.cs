using System;

namespace Marketplace.BusinessLogic.Adapter
{
    // ─── Clasa incompatibilă PayPal (Adaptee) ───────────────────────────────────
    /// <summary>
    /// API-ul original PayPal – are o interfață incompatibilă cu IPaymentGateway.
    /// </summary>
    public class PayPalApi
    {
        public string ApiVersion => "PayPal REST v2";

        public string CreateOrder(decimal total, string currencyCode)
        {
            var orderId = "PP-" + Guid.NewGuid().ToString()[..8].ToUpper();
            Console.WriteLine($"[PayPal] Order created: {orderId} for {total} {currencyCode}");
            return orderId;
        }

        public bool CapturePayment(string paypalOrderId)
        {
            Console.WriteLine($"[PayPal] Payment captured for order: {paypalOrderId}");
            return true;
        }

        public bool IssueRefund(string paypalOrderId, decimal refundAmount)
        {
            Console.WriteLine($"[PayPal] Refund of {refundAmount} issued for order: {paypalOrderId}");
            return true;
        }

        public string CheckOrderStatus(string paypalOrderId)
        {
            return "COMPLETED";
        }
    }

    // ─── Adaptor PayPal ──────────────────────────────────────────────────────────
    /// <summary>
    /// Adaptor care face PayPalApi compatibil cu IPaymentGateway.
    /// </summary>
    public class PayPalAdapter : IPaymentGateway
    {
        private readonly PayPalApi _payPal = new PayPalApi();

        public string GatewayName => "PayPal";

        public bool ProcessPayment(string orderId, decimal amount, string currency)
        {
            var paypalOrderId = _payPal.CreateOrder(amount, currency);
            return _payPal.CapturePayment(paypalOrderId);
        }

        public bool RefundPayment(string transactionId, decimal amount)
        {
            return _payPal.IssueRefund(transactionId, amount);
        }

        public string GetTransactionStatus(string transactionId)
        {
            return _payPal.CheckOrderStatus(transactionId);
        }
    }
}
