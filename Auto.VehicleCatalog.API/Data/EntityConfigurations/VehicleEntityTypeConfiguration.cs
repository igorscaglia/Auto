using Auto.VehicleCatalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auto.VehicleCatalog.API.Data.EntityConfigurations
{
    public class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        // Configure how physical data model will be created in database:
        // A Vehicle have an Id, Value, BrandId, ModelId, YearModel and 
        // a Fuelwhere Id is auto increment, all fields are required  and 
        // the BrandId and ModelId are foreign keys to Brands and Models table respectively.
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            // force do not pluralize table
            builder.ToTable("Vehicle");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(v => v.Value)
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(v => v.YearModel)
                .IsRequired();

            builder.Property(v => v.Fuel)
                .IsRequired()
                .HasMaxLength(20);

            // Fk for Brand table
            builder.HasOne(m => m.Brand)
                .WithMany()
                .HasForeignKey(fk => fk.BrandId)
                .IsRequired();

            // Fk for Model table
            builder.HasOne(m => m.Model)
                .WithMany()
                .HasForeignKey(fk => fk.ModelId)
                .IsRequired();
        }
    }
}