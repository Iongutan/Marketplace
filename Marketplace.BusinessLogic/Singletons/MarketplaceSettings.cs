using System;

namespace Marketplace.BusinessLogic.Singletons
{
    public sealed class MarketplaceSettings
    {
        private static readonly Lazy<MarketplaceSettings> _instance =
            new Lazy<MarketplaceSettings>(() => new MarketplaceSettings());

        public static MarketplaceSettings Instance => _instance.Value;

        private MarketplaceSettings()
        {
            // Setări implicite
            SiteName = "Premium Tech Marketplace";
            Currency = "RON";
            LastUpdated = DateTime.Now;
        }

        public string SiteName { get; set; } = "Marketplace";
        public string Currency { get; set; } = "RON";
        public DateTime LastUpdated { get; set; }

        // New properties for better utility
        public int MaxProductsPerUser { get; set; } = 10;
        public string DefaultCurrency { get; set; } = "MDL";
        public bool AllowGuestCheckout { get; set; } = false;
        public int MaxImageSizeMB { get; set; } = 5;

        public string GetInfo()
        {
            return $"Site: {SiteName}, Default Currency: {DefaultCurrency}, Max Products: {MaxProductsPerUser}";
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(SiteName))
                throw new InvalidOperationException("SiteName cannot be empty.");

            if (MaxProductsPerUser <= 0)
                throw new InvalidOperationException("MaxProductsPerUser must be greater than 0.");

            if (MaxImageSizeMB <= 0)
                throw new InvalidOperationException("MaxImageSizeMB must be greater than 0.");
        }
    }
}
