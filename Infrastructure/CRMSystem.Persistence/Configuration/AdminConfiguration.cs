// AdminConfiguration.cs
using CRMSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Surname)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(100);

            builder.Property(x => x.FinCode)
                   .HasMaxLength(20);

            // Relation: Admin → Job (Customer list in Job)
            builder.HasOne(x => x.Job)
                   .WithMany(x => x.Customers)
                   .HasForeignKey(x => x.JobId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Relation: Admin → SectionCustomer (bir adminin çoxlu Sections)
            builder.HasMany(x => x.Sections)
                   .WithOne(x => x.Admin)
                   .HasForeignKey(x => x.AdminId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
