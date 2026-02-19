# Script de configurare inițială pentru Marketplace

Write-Host "=== Configurare Marketplace ===" -ForegroundColor Cyan
Write-Host ""

# Verifică dacă dotnet este instalat
Write-Host "Verificare .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ .NET SDK $dotnetVersion găsit" -ForegroundColor Green
} else {
    Write-Host "✗ .NET SDK nu este instalat!" -ForegroundColor Red
    exit 1
}

# Restore pachete NuGet
Write-Host ""
Write-Host "Restaurare pachete NuGet..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Pachete restaurate cu succes" -ForegroundColor Green
} else {
    Write-Host "✗ Eroare la restaurarea pachetelor" -ForegroundColor Red
    exit 1
}

# Build proiectele
Write-Host ""
Write-Host "Build proiecte..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Build realizat cu succes" -ForegroundColor Green
} else {
    Write-Host "✗ Eroare la build" -ForegroundColor Red
    exit 1
}

# Verifică EF Core tools
Write-Host ""
Write-Host "Verificare EF Core tools..." -ForegroundColor Yellow
$efVersion = dotnet ef --version 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ EF Core tools instalat: $efVersion" -ForegroundColor Green
} else {
    Write-Host "! EF Core tools nu este instalat. Instalare..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ EF Core tools instalat cu succes" -ForegroundColor Green
    } else {
        Write-Host "✗ Eroare la instalarea EF Core tools" -ForegroundColor Red
    }
}

# Verifică SQL Server LocalDB
Write-Host ""
Write-Host "Verificare SQL Server LocalDB..." -ForegroundColor Yellow
$sqllocaldb = sqllocaldb info mssqllocaldb 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ SQL Server LocalDB găsit" -ForegroundColor Green
    
    # Pornește LocalDB dacă nu rulează
    sqllocaldb start mssqllocaldb 2>&1 | Out-Null
    Write-Host "✓ LocalDB pornit" -ForegroundColor Green
} else {
    Write-Host "! SQL Server LocalDB nu este găsit" -ForegroundColor Yellow
    Write-Host "  Instalează SQL Server Express cu LocalDB de la:" -ForegroundColor Yellow
    Write-Host "  https://aka.ms/ssmsfullsetup" -ForegroundColor Cyan
}

# Creare migrări (dacă nu există)
Write-Host ""
Write-Host "Verificare migrări bază de date..." -ForegroundColor Yellow
Set-Location Marketplace.Web

$migrationsFolder = "Migrations"
if (Test-Path $migrationsFolder) {
    Write-Host "✓ Migrări existente găsite" -ForegroundColor Green
} else {
    Write-Host "! Creare migrare inițială..." -ForegroundColor Yellow
    dotnet ef migrations add InitialCreate
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Migrare creată cu succes" -ForegroundColor Green
    } else {
        Write-Host "✗ Eroare la crearea migrării" -ForegroundColor Red
    }
}

# Aplicare migrări
Write-Host ""
Write-Host "Aplicare migrări la baza de date..." -ForegroundColor Yellow
dotnet ef database update
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Baza de date actualizată cu succes" -ForegroundColor Green
} else {
    Write-Host "✗ Eroare la actualizarea bazei de date" -ForegroundColor Red
}

Set-Location ..

Write-Host ""
Write-Host "=== Configurare completă! ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Pentru a rula aplicația:" -ForegroundColor Yellow
Write-Host "  1. Apasă F5 în VS Code" -ForegroundColor White
Write-Host "  2. Selectează 'Launch Marketplace.Web' sau 'Launch Design Patterns Demo'" -ForegroundColor White
Write-Host ""
Write-Host "Sau folosește comenzile:" -ForegroundColor Yellow
Write-Host "  cd Marketplace.Web && dotnet run" -ForegroundColor White
Write-Host "  cd Marketplace.DesignPatterns.Demo && dotnet run" -ForegroundColor White
Write-Host ""
