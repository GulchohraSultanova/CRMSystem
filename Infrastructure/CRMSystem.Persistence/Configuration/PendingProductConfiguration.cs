using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class PendingProductConfiguration : IEntityTypeConfiguration<PendingProduct>
    {
        public void Configure(EntityTypeBuilder<PendingProduct> builder)
        {
            builder.HasKey(x => x.Id);

            // Yeni dəyərlər üçün
            builder.Property(x => x.NewName)
                   .HasMaxLength(100);
            builder.Property(x => x.NewMeasure)
                 .HasMaxLength(10);



            // Original Product ilə əlaqə
            builder.Property(x => x.OriginalProductId)
                   .IsRequired();

            builder.HasOne(x => x.OriginalProduct)
                   .WithMany()
                   .HasForeignKey(x => x.OriginalProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Statuslar



            // BaseEntity propertiləri
            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.Property(x => x.LastUpdatedDate)
                   .IsRequired();

            builder.Property(x => x.DeletedDate)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .IsRequired();
        }
    }
}
