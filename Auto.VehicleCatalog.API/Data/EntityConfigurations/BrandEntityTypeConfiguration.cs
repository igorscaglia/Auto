using Auto.VehicleCatalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auto.VehicleCatalog.API.Data.EntityConfigurations
{
    public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
    {
        // Configure how physical data model will be created in database:
        // A brand have an Id and a Name where Id is auto increment and Name is required 
        // and unique
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            // force do not pluralize table
            builder.ToTable("Brand");

            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // create unique constraint on Name
            builder.HasIndex(b => b.Name)
                .IsUnique();
            builder.Property(cb => cb.Name)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}