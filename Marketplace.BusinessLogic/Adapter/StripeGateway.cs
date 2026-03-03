using System;

namespace Marketplace.BusinessLogic.Adapter
{
    // ─── Clasa incompatibilă Stripe (Adaptee) ───────────────────────────────────
    /// <summary>
    /// API-ul original Stripe – are o interfață distinctă de IPaymentGateway.
    /// </summary>
    public class StripeApi
    {
        public string ApiVersion => "Stripe API v3";

        public string CreatePaymentIntent(long amountCents, string currency)
        {
            var intentId = "pi_" + Guid.NewGuid().ToString("N")[..16];
            Console.WriteLine($"[Stripe] PaymentIntent {intentId} created for {amountCents} {currency.ToUpper()} cents");
            return intentId;
        }

        public bool ConfirmPaymentIntent(string paymentIntentId)
        {
            Console.WriteLine($"[Stripe] PaymentIntent {paymentIntentId} confirmed");
            return true;
        }

        public bool CreateRefund(string paymentIntentId, long amountCents)
        {
            Console.WriteLine($"[Stripe] Refund of {amountCents} cents for intent {paymentIntentId}");
            return true;
        }

        public string RetrievePaymentIntent(string paymentIntentId)
        {
            return "succeeded";
        }
    }

    // ─── Adaptor Stripe ──────────────────────────────────────────────────────────
    /// <summary>
    /// Adaptor care face StripeApi compatibil cu IPaymentGateway.
    /// Stripe lucrează cu cenți (long), deci efectuăm conversia.
    /// </summary>
    public class StripeAdapter : IPaymentGateway
    {
        private readonly StripeApi _stripe = new StripeApi();

        public string GatewayName => "Stripe";

        public bool ProcessPayment(string orderId, decimal amount, string currency)
        {
            // Stripe lucrează cu cenți
            long amountCents = (long)(amount * 100);
            var intentId = _stripe.CreatePaymentIntent(amountCents, currency);
            return _stripe.ConfirmPaymentIntent(intentId);
        }

        public bool RefundPayment(string transactionId, decimal amount)
        {
            long amountCents = (long)(amount * 100);
            return _stripe.CreateRefund(transactionId, amountCents);
        }

        public string GetTransactionStatus(string transactionId)
        {
            return _stripe.RetrievePaymentIntent(transactionId);
        }
    }
}
