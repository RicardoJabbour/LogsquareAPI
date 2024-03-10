using Microsoft.EntityFrameworkCore;
using Model.Data.Models;

namespace Model.Data
{
    public class LogsquareDbContext : DbContext
    {
        public LogsquareDbContext(DbContextOptions<LogsquareDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }


    }
}
