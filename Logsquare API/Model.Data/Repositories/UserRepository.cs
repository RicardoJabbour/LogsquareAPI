using Microsoft.EntityFrameworkCore;
using Model.Data.Models;
using Model.Data.Repositories.Interfaces;

namespace Model.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LogsquareDbContext _dbContext;

        public UserRepository(LogsquareDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public async Task<List<User>> AddUser(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var existUser = await _dbContext.Users.AnyAsync(u => u.Email == user.Email);
            if (existUser)
                throw new Exception("User Already Exists");

            _dbContext.Users.Add(user);
            var rowCount = await _dbContext.SaveChangesAsync();

            return rowCount > 0
                ? await _dbContext.Users.Select(x => x).ToListAsync()
                : throw new Exception("Add Error for the User " + user.Name);
                   
        }

        public async Task<List<User>> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user is User)
            {
                _dbContext.Users.Remove(user);
                var rowCount = _dbContext.SaveChanges();

                return rowCount > 0
                    ? await _dbContext.Users.Select(x => x).ToListAsync()
                    : throw new Exception("Delete Error for the User " + user.Id); ;
            }

            return null;
        }

        public async Task<User> GetUser(string email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                return user;
            }
            else
                return null;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.Select(x => x).ToListAsync();
        }

        public async Task<User> SignIn(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var existUser = await _dbContext.Users.AnyAsync(u => u.Email == user.Email);
            if (existUser)
                throw new Exception("User Already Exists");

            _dbContext.Users.Add(user);
            var rowCount = await _dbContext.SaveChangesAsync();

            return rowCount > 0
                ? user
                : throw new Exception("SignIn Error for the User " + user.Name);
        }

        public async Task<List<User>> UpdateUser(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var userToUpdate = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if(userToUpdate is User)
            {
                userToUpdate.Age = user.Age;
                userToUpdate.Email = user.Email;
                userToUpdate.Name = user.Name;
                userToUpdate.Phone = user.Phone;

                _dbContext.Users.Update(userToUpdate);
                var rowNB = _dbContext.SaveChanges();

                return rowNB > 0 ?
                    await _dbContext.Users.Select(x => x).ToListAsync() :
                    throw new Exception("Update Error for the User " + userToUpdate.Id);
            }

            return null;
        }
    }
}
