using System;

namespace Marketplace.Domain.Entities
{
    public class DigitalProduct : Product
    {
        public new string GetEntityDetails()
        {
            return $"Digital Product: {Name} - Price: {Price:C} (Stock: {Stock}) - Link: {DownloadUrl}";
        }

        public string DownloadUrl { get; set; }
        public string FileFormat { get; set; }
    }
}
