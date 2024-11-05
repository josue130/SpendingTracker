using Microsoft.EntityFrameworkCore;
using SpendingTracker.Domain.Entities;


namespace SpendingTracker.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Users> users { get; set; }
        public DbSet<Accounts> accounts { get; set; }
        public DbSet<UserAccounts> userAccounts { get; set; }
        public DbSet<CategoryIncome> categoryIncome { get; set; }
        public DbSet<Income> income { get; set; }
        public DbSet<CategoryExpense> categoryExpense { get; set; }
        public DbSet<Expense> expense { get; set; }
        public DbSet<MonthlyBalances> monthlyBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
        }
    }
}
