using System;

namespace Marketplace.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    
        public virtual string GetEntityDetails()
        {
            return $"Entity ID: {Id}, Created: {CreatedDate}";
        }
    }
}
