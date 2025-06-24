using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class AdminNotificationConfiguration : IEntityTypeConfiguration<AdminNotification>
    {
        public void Configure(EntityTypeBuilder<AdminNotification> builder)
        {
            builder.HasKey(x => x.Id);

            // “Type” is required and limited to e.g. 100 chars
            builder.Property(x => x.Type)
                   .IsRequired()
                   .HasMaxLength(100);

            // Optional foreign‐key to Category
            builder.Property(x => x.CategoryId).IsRequired(false);
            builder.HasOne(x => x.Category)
                   .WithMany() // we are not adding a navigation collection on Category side
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.OrderId).IsRequired(false);
            builder.HasOne(x => x.Order)
                   .WithMany() // we are not adding a navigation collection on Category side
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Optional foreign‐key to Product
            builder.Property(x => x.ProductId).IsRequired(false);
            builder.HasOne(x => x.Product)
                   .WithMany() // we are not adding a navigation collection on Product side
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);

            // “IsRead” flag
            builder.Property(x => x.IsRead)
                   .IsRequired();

        }
    }
}
