// SectionCustomerConfiguration.cs
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class SectionCustomerConfiguration : IEntityTypeConfiguration<SectionCustomer>
    {
        public void Configure(EntityTypeBuilder<SectionCustomer> builder)
        {
            builder.HasKey(x => x.Id);

            // FK to Section
            builder.Property(x => x.SectionId)
                   .IsRequired();
            builder.HasOne(x => x.Section)
                   .WithMany(x => x.CustomerList)
                   .HasForeignKey(x => x.SectionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // FK to Admin
            builder.Property(x => x.AdminId)
                   .IsRequired();
            builder.HasOne(x => x.Admin)
                   .WithMany(x => x.Sections)
                   .HasForeignKey(x => x.AdminId)
                   .OnDelete(DeleteBehavior.Cascade);

   
        }
    }
}
