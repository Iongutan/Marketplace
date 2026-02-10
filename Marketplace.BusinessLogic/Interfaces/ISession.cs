using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Interfaces
{
    public interface ISession
    {
        User GetCurrentUser();
    }
}
