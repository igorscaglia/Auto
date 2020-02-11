using Auto.VehicleCatalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auto.VehicleCatalog.API.Data.EntityConfigurations
{
    public class UserRoleEntityTypeConfiguration: IEntityTypeConfiguration<UserRole>
    {
        // Configure how physical data model will be created in database:
        // 
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // The primary key is combined
            var key = builder.HasKey(e => new { e.UserId, e.RoleId  });
            
            // Fk for Role table
            builder.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey( fk => fk.RoleId)
                .IsRequired();
            
            // Fk for User table
            builder.HasOne(e => e.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey( fk => fk.UserId)
                .IsRequired();
        }
    }
}