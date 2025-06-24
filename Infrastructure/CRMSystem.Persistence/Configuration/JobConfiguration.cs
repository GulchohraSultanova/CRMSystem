using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.JobImage)
                   .HasMaxLength(255);

            // ?? Section il? ?laq? (many-to-one)
  // v? ya Restrict, ehtiyacdan as?l? olaraq

            // ?? Adminl?r il? ?laq? (one-to-many)
            builder.HasMany(j => j.Customers)
                   .WithOne(a => a.Job)
                   .HasForeignKey(a => a.JobId)
                   .OnDelete(DeleteBehavior.Restrict); // Admin silinm?sin dey?
        }
    }
}
