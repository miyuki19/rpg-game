using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace rpg_game.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();
    }
}