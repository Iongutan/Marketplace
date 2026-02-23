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
            SiteName = "Marketplace";
            Currency = "RON";
            LastUpdated = DateTime.Now;
        }

        public string SiteName { get; set; }
        public string Currency { get; set; }
        public DateTime LastUpdated { get; set; }

        public string GetInfo()
        {
            return $"Site: {SiteName}, Currency: {Currency}, Last Updated: {LastUpdated}";
        }
    }
}
