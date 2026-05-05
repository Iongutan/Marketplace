using System;
using System.Linq;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;

namespace Marketplace.Data
{
    public static class DbSeeder
    {
        public static void SeedData(BusinessContext context)
        {
            // Asigură-te că baza de date a fost creată
            context.Database.EnsureCreated();

            // 1. Verificăm dacă există deja utilizatori în baza de date
            if (!context.Users.Any())
            {
                var users = new User[]
                {
                    new User { Username = "admin", Email = "admin@marketplace.com", Password = "password123", Role = UserRole.Admin, CreatedDate = DateTime.Now },
                    new User { Username = "client1", Email = "client1@test.com", Password = "clientpassword", Role = UserRole.Client, CreatedDate = DateTime.Now },
                    new User { Username = "client2", Email = "client2@test.com", Password = "clientpassword", Role = UserRole.Client, CreatedDate = DateTime.Now }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            // Produsele NU se seederesc din cod — vin exclusiv din baza de date SQL.
        }
    }
}
