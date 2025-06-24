// SectionConfiguration.cs
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.SectionImage)
                   .HasMaxLength(255);

            // Relation: Section → Department
            builder.HasOne(x => x.Department)
                   .WithMany(x => x.Sections)
                   .HasForeignKey(x => x.DepartmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relation: Section → SectionCustomer (bir Section-in çoxlu müştərisi)
            builder.HasMany(x => x.CustomerList)
                   .WithOne(x => x.Section)
                   .HasForeignKey(x => x.SectionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
