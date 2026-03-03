using System;

namespace Marketplace.BusinessLogic.Facade
{
    /// <summary>
    /// FAÇADE — Plasarea unei comenzi pe marketplace printr-o singură metodă,
    /// ascunzând complexitatea celor 4 subsisteme interne:
    ///   1. StockService             — verifică și rezervă stocul produsului
    ///   2. PaymentProcessorService  — procesează plata prin gateway ales
    ///   3. OrderRegistrationService — înregistrează comanda în sistem
    ///   4. OrderNotificationService — trimite confirmări cumpărătorului și vânzătorului
    ///
    /// Fără Façade, controller-ul ar apela manual toate 4 subsistemele.
    /// Cu Façade, apelează o singură metodă: PlaceOrder(...)
    /// </summary>
    public class OrderFacade
    {
        private readonly StockService _stock = new();
        private readonly PaymentProcessorService _payment = new();
        private readonly OrderRegistrationService _registration = new();
        private readonly OrderNotificationService _notification = new();

        /// <summary>
        /// Plasează o comandă pe marketplace orchestrând automat toate subsistemele.
        /// </summary>
        public OrderResult? PlaceOrder(
            int productId,
            string productName,
            decimal pricePerUnit,
            bool isDigital,
            int sellerId,
            string buyerName,
            string buyerEmail,
            int quantity = 1,
            string paymentGateway = "PayPal")
        {
            Console.WriteLine($"\n=== ORDER FACADE: Procesare comandă pentru '{buyerName}' ===");

            // Pasul 1: Verificare stoc (subsistem intern)
            if (!_stock.CheckStock(productId, quantity, productName))
            {
                Console.WriteLine("[Facade] Stoc insuficient. Comanda anulată.");
                return null;
            }
            _stock.ReserveStock(productId, quantity, productName);

            decimal totalAmount = pricePerUnit * quantity;

            // Pasul 2: Procesare plată (subsistem intern)
            string transactionId = _payment.ProcessPayment(buyerName, totalAmount, paymentGateway);

            // Pasul 3: Înregistrare comandă (subsistem intern)
            string orderNumber = _registration.RegisterOrder(productId, buyerName, quantity, totalAmount);

            // Pasul 4: Construire rezultat
            var result = new OrderResult
            {
                OrderNumber = orderNumber,
                ProductName = productName,
                BuyerName = buyerName,
                BuyerEmail = buyerEmail,
                Quantity = quantity,
                TotalAmount = totalAmount,
                TransactionId = transactionId,
                PaymentGateway = paymentGateway,
                IsDigital = isDigital,
                OrderDate = DateTime.Now
            };

            // Pasul 5: Notificări (subsistem intern)
            _notification.NotifyBuyer(buyerEmail, result);
            _notification.NotifySeller(sellerId, orderNumber, productName);

            Console.WriteLine($"=== ORDER FACADE: Comanda {orderNumber} finalizată! ===\n");
            return result;
        }
    }
}
