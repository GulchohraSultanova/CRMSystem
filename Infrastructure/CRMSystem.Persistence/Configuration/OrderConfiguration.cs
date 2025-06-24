using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.EmployeeConfirm)
                   .HasDefaultValue(false);

            builder.Property(x => x.FighterConfirm)
                   .HasDefaultValue(false);

            builder.Property(x => x.EmployeeDelivery)
                   .HasDefaultValue(false);

            builder.Property(x => x.OrderDeliveryTime)
                   .IsRequired();

            builder.Property(x => x.OrderLimitTime)
                   .IsRequired();

            // Order ? OrderItem (one-to-many)
            builder.HasMany(x => x.Items)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Order ? OrderOverhead (one-to-many)
            builder.HasMany(x => x.Overhead)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Order - Admin (Customer)
            builder.HasOne(x => x.Admin)
                   .WithMany()
                   .HasForeignKey(x => x.AdminId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Order - Admin (Fighter)
            builder.HasOne(x => x.Fighter)
                   .WithMany()
                   .HasForeignKey(x => x.FighterId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
