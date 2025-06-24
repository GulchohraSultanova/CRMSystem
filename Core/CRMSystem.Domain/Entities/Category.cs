using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string? CategoryImage { get; set; }
        public bool IsCreated { get; set; } = false;
        public bool Deleted { get; set; }=false;
        public bool IsUpdated {  get; set; } = false;
        public List<Product>? Products { get; set; }
    }
}
