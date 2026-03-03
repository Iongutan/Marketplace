using Microsoft.EntityFrameworkCore;
using Marketplace.Data;
using Marketplace.Data.Interfaces;
using Marketplace.Data.Implementations;
using Marketplace.BusinessLogic.Interfaces;
using Marketplace.BusinessLogic.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddDbContext<BusinessContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("MarketplaceDb")));

builder.Services.AddDbContext<BusinessContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MarketplaceDb")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductService, ProductApi>();
builder.Services.AddScoped<UserApi>();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Initialize Singleton from configuration
builder.Configuration.GetSection("MarketplaceSettings").Bind(Marketplace.BusinessLogic.Singletons.MarketplaceSettings.Instance);
Marketplace.BusinessLogic.Singletons.MarketplaceSettings.Instance.Validate();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
