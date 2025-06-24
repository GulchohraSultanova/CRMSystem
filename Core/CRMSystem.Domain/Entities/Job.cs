using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class Job:BaseEntity
    {
        public string Name { get; set; }
        public string ? JobImage {  get; set; }

        public List<Admin>? Customers { get; set; }
    }
}
