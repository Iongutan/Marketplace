using System;

using Marketplace.Domain.Interfaces;

namespace Marketplace.Domain.Entities
{
    public class Product : BaseEntity, IPrototype<Product>
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

        public Product Clone()
        {
            var clone = (Product)this.MemberwiseClone();
            clone.Id = 0;
            clone.CreatedDate = DateTime.Now;
            clone.Name = "Copie a " + this.Name;
            return clone;
        }

        public Product CloneAsTemplate()
        {
            var clone = (Product)this.MemberwiseClone();
            clone.Id = 0;
            clone.CreatedDate = DateTime.Now;
            clone.Stock = 0;
            // Name remains original
            return clone;
        }

        public override string GetEntityDetails()
        {
            return $"Product: {Name} - Brand: {Brand} - Price: {Price?.ToString("C") ?? "N/A"} (Stock: {Stock?.ToString() ?? "0"})";
        }
    }
}
