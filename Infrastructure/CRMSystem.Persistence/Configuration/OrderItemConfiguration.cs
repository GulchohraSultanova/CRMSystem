// OrderItemConfiguration.cs
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)").HasDefaultValue(0m);

            builder.Property(x => x.RequiredQuantity)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)").HasDefaultValue(0m);

            builder.Property(x => x.SuppliedQuantity)
                   .HasColumnType("decimal(18,2)").HasDefaultValue(0m);

       

            // OrderItem → Product (many-to-one)
            builder.HasOne(x => x.Product)
                   .WithMany() // Product tarafında koleksiyon yoxdursa
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            // OrderItem → Order (many-to-one)
            builder.HasOne(x => x.Order)
                   .WithMany(x => x.Items)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // OrderItem → Vendor (many-to-one, optional)
            builder.HasOne(x => x.Vendor)
                   .WithMany() // Vendor tarafında koleksiyon yoxdursa
                   .HasForeignKey(x => x.VendorId)
                   .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Vendor)
         .WithMany(v => v.OrderItems)
         .HasForeignKey(x => x.VendorId)
         .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
