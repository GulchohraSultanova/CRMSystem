// OrderOverheadConfiguration.cs
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class OrderOverheadConfiguration : IEntityTypeConfiguration<OrderOverhead>
    {
        public void Configure(EntityTypeBuilder<OrderOverhead> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            // OrderOverhead ? Order (many-to-one)
            builder.HasOne(x => x.Order)
                   .WithMany(x => x.Overhead)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
