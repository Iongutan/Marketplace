# Notă Importantă: Migrări EF Core cu .NET 10 Preview

## Problemă Identificată
Există o problemă cunoscută cu EF Core migrations în .NET 10 Preview:
```
Could not load assembly 'Marketplace.Web'. Ensure it is referenced by the startup project 'Marketplace.Web'.
```

## Soluții Alternative

### Opțiunea 1: Folosește InMemory Database (Recomandat pentru Development)
Proiectul deja are `Microsoft.EntityFrameworkCore.InMemory` instalat în `Marketplace.Data`.

**Modifică `Program.cs` în Marketplace.Web:**
```csharp
// Comentează linia existentă:
// builder.Services.AddDbContext<BusinessContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("MarketplaceDb")));

// Adaugă în loc:
builder.Services.AddDbContext<BusinessContext>(options =>
    options.UseInMemoryDatabase("MarketplaceDb"));
```

**Avantaje:**
- ✓ Nu necesită SQL Server LocalDB
- ✓ Nu necesită migrări
- ✓ Perfect pentru testing și development
- ✓ Funcționează imediat

### Opțiunea 2: Creează Manual Migrările
Dacă vrei neapărat SQL Server, poți crea manual fișierele de migrare:

1. Creează folder `Migrations` în `Marketplace.Web`
2. Adaugă fișier `20260217_InitialCreate.cs`:

```csharp
using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Web.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Stock = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    // Digital Product fields
                    DownloadUrl = table.Column<string>(nullable: true),
                    FileFormat = table.Column<string>(nullable: true),
                    FileSize = table.Column<long>(nullable: true),
                    // Physical Product fields
                    Weight = table.Column<double>(nullable: true),
                    Dimensions = table.Column<string>(nullable: true),
                    ShippingCost = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Products");
            migrationBuilder.DropTable(name: "Users");
        }
    }
}
```

### Opțiunea 3: Așteaptă .NET 10 RTM
Problema va fi rezolvată în versiunea finală de .NET 10.

## Recomandare
**Pentru moment, folosește Opțiunea 1 (InMemory Database)** - este cea mai simplă și funcționează perfect pentru development și testing.
