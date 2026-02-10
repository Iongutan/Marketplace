using Marketplace.Data.Interfaces;
using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Core
{
    public class UserApi
    {
         private readonly IRepository<User> _userRepository;

        public UserApi(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        
        // Add user logic here
    }
}
