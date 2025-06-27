using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class Company:BaseEntity
    {
        public string Name { get; set; }
        public string? CompanyImage { get; set; }
        public List<Department>? Departments { get; set; }
        public List<MonthlyCalculation>? Calculations { get; set; }

    }
}
