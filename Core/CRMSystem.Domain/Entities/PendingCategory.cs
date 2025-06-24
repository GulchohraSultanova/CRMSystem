using CRMSystem.Domain.Entities.Common;
using System;

namespace CRMSystem.Domain.Entities
{
    public class PendingCategory : BaseEntity
    {
        public Guid OriginalCategoryId { get; set; }
        public Category? OriginalCategory { get; set; }

        // Yeni göndərilən dəyərlər (update üçün)
        public string? NewName { get; set; }


        // Sadəcə update üçün istifadə olunur

    }
}
