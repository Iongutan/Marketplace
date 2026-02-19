# Marketplace - Configurare și Rulare

## Proiecte Disponibile

Acest workspace conține două proiecte principale:

### 1. **Marketplace.Web** - Aplicație Web ASP.NET Core
Aplicație web cu interfață pentru utilizatori și administratori.

### 2. **Marketplace.DesignPatterns.Demo** - Aplicație Consolă
Demonstrație a pattern-urilor de design (Factory Method și Abstract Factory).

---

## Cum să Rulezi Proiectele

### Opțiunea 1: Folosind VS Code Debugger (Recomandat)

1. Apasă **F5** sau mergi la **Run and Debug** (Ctrl+Shift+D)
2. Selectează configurația dorită:
   - **Launch Marketplace.Web** - pentru aplicația web
   - **Launch Design Patterns Demo** - pentru demo-ul console

### Opțiunea 2: Folosind Terminal

#### Pentru Aplicația Web:
```powershell
cd Marketplace.Web
dotnet run
```
Aplicația va porni pe `https://localhost:5001` (sau portul afișat în terminal).

#### Pentru Demo Design Patterns:
```powershell
cd Marketplace.DesignPatterns.Demo
dotnet run
```

---

## Configurare Bază de Date

### Pasul 1: Verifică SQL Server LocalDB
Asigură-te că ai SQL Server LocalDB instalat (vine cu Visual Studio).

### Pasul 2: Creează Migrările (dacă nu există)
```powershell
cd Marketplace.Web
dotnet ef migrations add InitialCreate
```

### Pasul 3: Aplică Migrările
```powershell
dotnet ef database update
```

### Connection String
Baza de date este configurată în `appsettings.json`:
```
Server=(localdb)\\mssqllocaldb;Database=MarketplaceDb;Trusted_Connection=True
```

---

## Structura Proiectului

```
marketplace/
├── Marketplace.Domain/          # Entități și interfețe
├── Marketplace.Data/            # DbContext și Repository
├── Marketplace.BusinessLogic/   # Logică de business și pattern-uri
├── Marketplace.Web/             # Aplicație web ASP.NET Core
└── Marketplace.DesignPatterns.Demo/  # Demo console pattern-uri
```

---

## Comenzi Utile

### Build toate proiectele:
```powershell
dotnet build
```

### Restore pachete NuGet:
```powershell
dotnet restore
```

### Clean build artifacts:
```powershell
dotnet clean
```

### Watch mode (auto-rebuild la modificări):
```powershell
cd Marketplace.Web
dotnet watch run
```

---

## Troubleshooting

### Eroare: "Cannot connect to database"
- Verifică dacă SQL Server LocalDB este pornit
- Rulează: `sqllocaldb start mssqllocaldb`

### Eroare: "dotnet ef command not found"
Instalează EF Core tools:
```powershell
dotnet tool install --global dotnet-ef
```

### Port-ul este deja folosit
Modifică portul în `Marketplace.Web/Properties/launchSettings.json`
