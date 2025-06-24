using CRMSystem.Application.Dtos.Department;
using CRMSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Company
{
    public class CompanyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

       
        public List<DepartmentDto>? Departments { get; set; }
    }
}
