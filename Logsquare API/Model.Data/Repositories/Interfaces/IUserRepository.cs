using Model.Data.Models;

namespace Model.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> AddUser(User user);
        public Task<User> GetUser(string email);
        public Task<List<User>> GetUsers();
        public Task<List<User>> UpdateUser(User user);
        public Task<List<User>> DeleteUser(int id);
        public Task<User> SignIn(User user);
    }
}
