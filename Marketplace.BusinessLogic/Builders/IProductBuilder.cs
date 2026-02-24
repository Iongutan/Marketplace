using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Builders
{
    public interface IProductBuilder
    {
        IProductBuilder Reset(bool isDigital);
        IProductBuilder SetName(string name);
        IProductBuilder SetDescription(string description);
        IProductBuilder SetPrice(decimal price);
        IProductBuilder SetCategory(string category);
        IProductBuilder SetBrand(string brand);
        IProductBuilder SetStock(int stock);
        IProductBuilder SetPhysicalDetails(decimal weight, decimal shippingCost);
        IProductBuilder SetDigitalDetails(string downloadUrl, string fileFormat);
        IProductBuilder SetImageUrl(string imageUrl);
        IProductBuilder SetUserId(int userId);
        IProductBuilder SetIsDigital(bool isDigital);
        Product Build();
    }
}
