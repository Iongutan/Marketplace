using System;
using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Observer
{
    public interface IObserver
    {
        void Update(string message);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public class ProductPriceSubject : ISubject
    {
        private readonly List<IObserver> _observers = new List<IObserver>();
        private decimal _price;
        public string ProductName { get; set; }

        public ProductPriceSubject(string productName, decimal initialPrice)
        {
            ProductName = productName;
            _price = initialPrice;
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    Notify();
                }
            }
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            string message = $"The price for {ProductName} has changed to {Price:C}";
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }

    public class CustomerObserver : IObserver
    {
        public string CustomerName { get; private set; }
        public string LastNotification { get; private set; }

        public CustomerObserver(string customerName)
        {
            CustomerName = customerName;
        }

        public void Update(string message)
        {
            LastNotification = $"To {CustomerName}: {message}";
            Console.WriteLine(LastNotification);
        }
    }
}
