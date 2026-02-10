using Microsoft.EntityFrameworkCore;
using Marketplace.Domain.Entities;

namespace Marketplace.Data
{
    public class BusinessContext : DbContext
    {
        public BusinessContext(DbContextOptions<BusinessContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
