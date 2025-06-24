using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMSystem.Persistence.Configurations
{
    public class PendingCategoryConfiguration : IEntityTypeConfiguration<PendingCategory>
    {
        public void Configure(EntityTypeBuilder<PendingCategory> builder)
        {
            builder.HasKey(x => x.Id);

            // Yeni dəyərlər üçün
            builder.Property(x => x.NewName)
                   .HasMaxLength(100);

         

            // Original category ilə əlaqə
            builder.Property(x => x.OriginalCategoryId)
                   .IsRequired();

            builder.HasOne(x => x.OriginalCategory)
                   .WithMany()
                   .HasForeignKey(x => x.OriginalCategoryId)
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
