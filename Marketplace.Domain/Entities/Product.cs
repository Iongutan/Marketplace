using System;

namespace Marketplace.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int? UserId { get; set; } // Proprietarul produsului
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsDigital { get; set; }

        public override string GetEntityDetails()
        {
            return $"Product: {Name} - Brand: {Brand} - Price: {Price?.ToString("C") ?? "N/A"} (Stock: {Stock?.ToString() ?? "0"})";
        }
    }
}
