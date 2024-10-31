using Microsoft.EntityFrameworkCore;
using SpendingTracker.Domain.entities;


namespace SpendingTracker.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Users> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
        }
    }
}
