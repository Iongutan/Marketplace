using System;

namespace Marketplace.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public override string GetEntityDetails()
        {
            return $"Product: {Name} - Price: {Price:C} (Stock: {Stock})";
        }
    }
}
