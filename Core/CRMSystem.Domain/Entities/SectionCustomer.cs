using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class SectionCustomer:BaseEntity
    {
        public Guid SectionId { get; set; }
        public string AdminId { get; set; }
        public Section? Section { get; set; }
        public Admin? Admin { get; set; }
    }
}
