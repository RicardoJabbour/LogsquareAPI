using Model.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
