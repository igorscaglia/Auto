using Auto.VehicleCatalog.API.Data.EntityConfigurations;
using Auto.VehicleCatalog.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auto.VehicleCatalog.API.Data
{
    public class DataContext : IdentityDbContext<
        User, Role, int, IdentityUserClaim<int>, UserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        protected DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model.Model> Models { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ASP.NET Core Identity requirement
            base.OnModelCreating(builder);

            // Applying Fluent API
            builder.ApplyConfiguration(new BrandEntityTypeConfiguration());
            builder.ApplyConfiguration(new ModelEntityTypeConfiguration());
            builder.ApplyConfiguration(new VehicleEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
        }
    }
}
