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
            if (user == null || string.IsNullOrEmpty(user.Username) ||
                string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                throw new Exception("Invalid user data. All fields are required.");

            var trimmedUsername = user.Username.Trim();
            var trimmedEmail = user.Email.Trim();

            // Simple validation: check if username/email exists (case-insensitive)
            var existingUser = _userRepository.GetAll().FirstOrDefault(u =>
                (!string.IsNullOrEmpty(u.Username) && u.Username.Trim().Equals(trimmedUsername, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(u.Email) && u.Email.Trim().Equals(trimmedEmail, StringComparison.OrdinalIgnoreCase)));

            if (existingUser != null)
            {
                throw new Exception("User with this username or email already exists.");
            }

            user.Username = trimmedUsername;
            user.Email = trimmedEmail;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Secure Password Hashing
            user.CreatedDate = DateTime.Now;

            _userRepository.Insert(user);
        }

        public User? ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var trimmedUsername = username.Trim();
            var trimmedPassword = password.Trim();

            var user = _userRepository.GetAll().FirstOrDefault(u =>
                !string.IsNullOrEmpty(u.Username) && u.Username.Trim().Equals(trimmedUsername, StringComparison.OrdinalIgnoreCase));

            if (user != null && !string.IsNullOrEmpty(user.Password))
            {
                // 1. Try BCrypt validation
                try
                {
                    if (user.Password.StartsWith("$2") && BCrypt.Net.BCrypt.Verify(trimmedPassword, user.Password))
                    {
                        return user;
                    }
                }
                catch (BCrypt.Net.SaltParseException)
                {
                    // Fallback to plain text if salt is invalid
                }

                // 2. Fallback to plain text (for existing DB users)
                if (user.Password.Equals(trimmedPassword))
                {
                    return user;
                }
            }

            return null;
        }
    }
}
