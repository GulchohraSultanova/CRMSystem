using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class PendingProduct:BaseEntity
    {
        public Guid OriginalProductId { get; set; }
        public Product? OriginalProduct{ get; set; }

        // Yeni göndərilən dəyərlər (update üçün)
        public string? NewName { get; set; }
        public string? NewMeasure { get; set; }
    }
}
