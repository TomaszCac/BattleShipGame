using BattleShipGame.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleShipGame.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
