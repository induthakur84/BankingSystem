using Account.Domain;
using Microsoft.EntityFrameworkCore;

namespace Account.Data.Context
{
    public class AccountDBContext:DbContext
    {

        public AccountDBContext(DbContextOptions<AccountDBContext> options) : base(options) { }
           
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountDBContext).Assembly);
        }

    }
}
