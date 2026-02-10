using System;
using Marketplace.BusinessLogic.AbstractFactory;
using Marketplace.BusinessLogic.Factories;
using Marketplace.Domain.Entities;

namespace Marketplace.DesignPatterns.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Demonstrație Pattern-uri Creționale ===\n");

            // 1. Factory Method Demo
            Console.WriteLine("--- Factory Method: Creare Produse ---");

            ProductFactory physicalFactory = new PhysicalProductFactory();
            Product physicalProduct = physicalFactory.CreateProduct("Laptop Gaming", 3500.00m, 10);
            physicalProduct.CreatedDate = DateTime.Now; // Logic moved from factory
            Console.WriteLine(physicalProduct.GetEntityDetails());

            ProductFactory digitalFactory = new DigitalProductFactory();
            Product digitalProduct = digitalFactory.CreateProduct("E-Book C# Advanced", 50.00m, 1000);
            digitalProduct.CreatedDate = DateTime.Now; // Logic moved from factory
            Console.WriteLine(digitalProduct.GetEntityDetails());

            Console.WriteLine("\n--------------------------------------\n");

            // 2. Abstract Factory Demo (Marketplace Fulfillment)
            Console.WriteLine("--- Abstract Factory: Marketplace Fulfillment ---");

            Console.WriteLine("Select Product Type (1: Physical, 2: Digital): ");
            // Simulating choice for demo purposes
            string typeChoice = "1";

            IOrderFactory orderFactory;
            if (typeChoice == "1")
            {
                Console.WriteLine("Payment Method (1: Card, 2: Cash): ");
                string paymentInput = "2"; // Simulating Cash choice
                string paymentMethodType = paymentInput == "2" ? "Cash" : "Card";

                orderFactory = new PhysicalOrderFactory(paymentMethodType);
                Console.WriteLine($"Processing Physical Order via {paymentMethodType}...");
            }
            else
            {
                orderFactory = new DigitalOrderFactory();
                Console.WriteLine("Processing Digital Order...");
            }

            // Create family of services
            IShippingMethod shippingMethod = orderFactory.CreateShippingMethod();
            IPaymentMethod paymentMethod = orderFactory.CreatePaymentMethod();

            // Use services (Client doesn't know concrete types)
            paymentMethod.ProcessPayment(100.00m);
            shippingMethod.Ship(new object()); // Dummy order object

            Console.WriteLine("\nDone.");
        }
    }
}
