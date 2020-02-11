using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auto.VehicleCatalog.API.Data.EntityConfigurations
{
    public class ModelEntityTypeConfiguration: IEntityTypeConfiguration<Model.Model>
    {
        // Configure how physical data model will be created in database:
        // A Model have an Id, a Name and a BrandId where Id is auto increment, 
        // the name is required and unique and the BrandId is a foreign key to Brands table 
        // and should be required.
        public void Configure(EntityTypeBuilder<Model.Model> builder)
        {
            // force do not pluralize table
            builder.ToTable("Model");

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
            
            // Fk for Brand table
            builder.HasOne(m => m.Brand)
                .WithMany(r => r.Models)
                .HasForeignKey(fk => fk.BrandId)
                .OnDelete(DeleteBehavior.Cascade) // make sense be cascade ;-)
                .IsRequired();
        }
        
    }
}