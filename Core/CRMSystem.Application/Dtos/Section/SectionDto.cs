using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.Dtos.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Section
{
    public class SectionDto
    {
        public string Id {  get; set; }
        public string Name { get; set; }

        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyName { get; set; }
        

        public List<AdminInfoDto>? Customers { get; set; }
    }
}
