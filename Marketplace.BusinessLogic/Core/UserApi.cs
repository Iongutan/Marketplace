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

        public void Register(User user)
        {
            // Simple validation: check if username/email exists
            var existingUser = _userRepository.GetAll().FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }
            _userRepository.Insert(user);
        }

        public User ValidateUser(string username, string password)
        {
            return _userRepository.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
