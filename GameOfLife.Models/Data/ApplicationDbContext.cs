using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Generation> Generations { get; set; }
    }
}
