using System;

namespace Marketplace.BusinessLogic.Adapter
{
    // ─── Clasa incompatibilă Google Pay (Adaptee) ───────────────────────────────
    /// <summary>
    /// API-ul original Google Pay – are o interfață specifică Googlei.
    /// </summary>
    public class GooglePayApi
    {
        public string ApiVersion => "Google Pay API v2";

        public GooglePayToken RequestPaymentToken(string merchantId, double amount, string countryCode)
        {
            Console.WriteLine($"[GooglePay] Token requested from merchant {merchantId} for {amount} {countryCode}");
            return new GooglePayToken { Token = "GPT-" + Guid.NewGuid().ToString()[..8], IsValid = true };
        }

        public bool AuthorizeToken(GooglePayToken token)
        {
            Console.WriteLine($"[GooglePay] Token {token.Token} authorized");
            return token.IsValid;
        }

        public bool InitiateRefund(string tokenId, double refundAmount)
        {
            Console.WriteLine($"[GooglePay] Refund of {refundAmount} initiated for token {tokenId}");
            return true;
        }

        public string GetTokenState(string tokenId)
        {
            return "ACTIVE";
        }
    }

    public class GooglePayToken
    {
        public string Token { get; set; } = string.Empty;
        public bool IsValid { get; set; }
    }

    // ─── Adaptor Google Pay ──────────────────────────────────────────────────────
    /// <summary>
    /// Adaptor care face GooglePayApi compatibil cu IPaymentGateway.
    /// </summary>
    public class GooglePayAdapter : IPaymentGateway
    {
        private const string MerchantId = "MARKETPLACE_MERCHANT_001";
        private readonly GooglePayApi _googlePay = new GooglePayApi();

        public string GatewayName => "Google Pay";

        public bool ProcessPayment(string orderId, decimal amount, string currency)
        {
            // Google Pay returnează un token pe care îl autorizăm ulterior
            var token = _googlePay.RequestPaymentToken(MerchantId, (double)amount, currency);
            return _googlePay.AuthorizeToken(token);
        }

        public bool RefundPayment(string transactionId, decimal amount)
        {
            return _googlePay.InitiateRefund(transactionId, (double)amount);
        }

        public string GetTransactionStatus(string transactionId)
        {
            return _googlePay.GetTokenState(transactionId);
        }
    }
}
