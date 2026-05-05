using System;

namespace Marketplace.BusinessLogic.ChainOfResponsibility
{
    public enum SupportType
    {
        Delivery = 1,
        Return = 2,
        Store = 3
    }

    public class SupportRequest
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public SupportType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? HandledBy { get; set; }
        public string? Resolution { get; set; }
        public bool IsResolved { get; set; } = false;

        public SupportRequest(string customerName, string subject, string description, SupportType type)
        {
            Id = new Random().Next(1000, 9999);
            CustomerName = customerName;
            Subject = subject;
            Description = description;
            Type = type;
        }
    }
}
