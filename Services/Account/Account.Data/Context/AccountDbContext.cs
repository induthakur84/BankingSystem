using Account.Domain;
using Microsoft.EntityFrameworkCore;

namespace Account.Data.Context
{
    public class AccountDBContext:DbContext
    {

        public AccountDBContext(DbContextOptions<AccountDBContext> options) : base(options) { }
           
        public DbSet<User> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountDBContext).Assembly);
        }

    }
}
