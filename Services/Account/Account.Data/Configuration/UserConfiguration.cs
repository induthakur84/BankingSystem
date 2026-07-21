using Account.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
           builder.Property(u=>u.Name).IsRequired();
            builder.HasIndex(u => u.Name).IsUnique();
        }
    }
}
