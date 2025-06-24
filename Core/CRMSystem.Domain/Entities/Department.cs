using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class Department:BaseEntity
    {
        public string Name { get; set; }
        public string? DepartmentImage { get; set; }
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
        public List<Section>? Sections { get; set; }
    }
}
