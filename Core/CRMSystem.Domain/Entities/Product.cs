using CRMSystem.Domain.Entities.Common;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public bool IsCreated { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public bool IsUpdated { get; set; } = false;
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? ProductImage { get; set; }
        public string Measure { get; set; } = "kq";
    }
}
