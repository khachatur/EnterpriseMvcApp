using EnterpriseMvcApp.Models;
using EnterpriseMvcApp.Repositories;

namespace EnterpriseMvcApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _userRepository.GetAllUsersAsync();
        }

        public Task<User> GetUserByIdAsync(Guid id)
        {
            return _userRepository.GetUserByIdAsync(id);
        }

        public Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return _userRepository.GetUserByUsernameOrEmailAsync(usernameOrEmail);
        }

        public Task AddUserAsync(User user)
        {
            return _userRepository.AddUserAsync(user);
        }

        public Task UpdateUserAsync(User user)
        {
            return _userRepository.UpdateUserAsync(user);
        }

        public Task DeleteUserAsync(Guid id)
        {
            return _userRepository.DeleteUserAsync(id);
        }
    }
}
