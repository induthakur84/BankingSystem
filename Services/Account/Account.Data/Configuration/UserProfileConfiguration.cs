using Account.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Data.Configuration
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            //one to one relationship

            builder.HasOne(p => p.User)
                .WithOne(up => up.UserProfile)
                .HasForeignKey<UserProfile>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
           
        }
    }
}
