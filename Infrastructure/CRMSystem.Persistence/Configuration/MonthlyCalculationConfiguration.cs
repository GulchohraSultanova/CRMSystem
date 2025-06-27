using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class MonthlyCalculationConfiguration : IEntityTypeConfiguration<MonthlyCalculation>
    {
        public void Configure(EntityTypeBuilder<MonthlyCalculation> builder)
        {
            builder.ToTable("MonthlyCalculations");

            builder.HasKey(mc => mc.Id);

            builder.Property(mc => mc.Year)
                   .IsRequired();

            builder.Property(mc => mc.Month)
                   .IsRequired();

            builder.Property(mc => mc.InitialAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(mc => mc.TotalOrderAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            // TotalAmount is computed in C#, not mapped to DB (optional)
            builder.Ignore(mc => mc.TotalAmount);

            builder.HasOne(mc => mc.Company)
                   .WithMany(c => c.Calculations)
                   .HasForeignKey(mc => mc.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
