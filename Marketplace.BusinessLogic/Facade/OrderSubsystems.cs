using System;
using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Facade
{
    // ════════════════════════════════════════════════════════════════════════════
    //  Subsistem 1: Verificare și rezervare stoc
    // ════════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Subsistem intern — verifică dacă produsul are stoc disponibil.
    /// Utilizat exclusiv de OrderFacade, nu direct de controller.
    /// </summary>
    public class StockService
    {
        public bool CheckStock(int productId, int quantity, string productName)
        {
            Console.WriteLine($"[Stock] Verificare stoc pentru '{productName}' (ID={productId}), cantitate={quantity}...");
            return quantity > 0;
        }

        public void ReserveStock(int productId, int quantity, string productName)
        {
            Console.WriteLine($"[Stock] Rezervare {quantity} buc. din '{productName}'.");
        }
    }

    // ════════════════════════════════════════════════════════════════════════════
    //  Subsistem 2: Procesare plată
    // ════════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Subsistem intern — procesează plata comenzii prin gateway-ul ales.
    /// </summary>
    public class PaymentProcessorService
    {
        public string ProcessPayment(string buyerName, decimal amount, string gateway = "PayPal")
        {
            var transactionId = "MKT-" + Guid.NewGuid().ToString()[..8].ToUpper();
            Console.WriteLine($"[Payment] {amount:F2} MDL procesați prin {gateway} pentru '{buyerName}'. TX={transactionId}");
            return transactionId;
        }
    }

    // ════════════════════════════════════════════════════════════════════════════
    //  Subsistem 3: Înregistrare comandă
    // ════════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Subsistem intern — creează înregistrarea comenzii în sistem.
    /// </summary>
    public class OrderRegistrationService
    {
        public string RegisterOrder(int productId, string buyerName, int quantity, decimal totalAmount)
        {
            var orderNumber = "ORD-" + DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString()[..4].ToUpper();
            Console.WriteLine($"[Order] Comanda {orderNumber}: '{buyerName}' x{quantity} buc., total {totalAmount:F2} MDL.");
            return orderNumber;
        }
    }

    // ════════════════════════════════════════════════════════════════════════════
    //  Subsistem 4: Notificări
    // ════════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Subsistem intern — trimite confirmări cumpărătorului și vânzătorului.
    /// </summary>
    public class OrderNotificationService
    {
        public void NotifyBuyer(string email, OrderResult result)
        {
            Console.WriteLine($"[Notification] Confirmare trimisă la {email} — Comanda {result.OrderNumber}");
        }

        public void NotifySeller(int sellerId, string orderNumber, string productName)
        {
            Console.WriteLine($"[Notification] Vânzătorul (ID={sellerId}) notificat: comanda {orderNumber} pentru '{productName}'.");
        }
    }

    // ════════════════════════════════════════════════════════════════════════════
    //  DTO — Rezultatul comenzii
    // ════════════════════════════════════════════════════════════════════════════
    public class OrderResult
    {
        public string   OrderNumber    { get; set; } = string.Empty;
        public string   ProductName    { get; set; } = string.Empty;
        public string   BuyerName      { get; set; } = string.Empty;
        public string   BuyerEmail     { get; set; } = string.Empty;
        public int      Quantity       { get; set; }
        public decimal  TotalAmount    { get; set; }
        public string   TransactionId  { get; set; } = string.Empty;
        public string   PaymentGateway { get; set; } = string.Empty;
        public bool     IsDigital      { get; set; }
        public DateTime OrderDate      { get; set; }
    }
}
