using CRMSystem.Application.Dtos.Section;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Department
{
    public class DepartmentDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string CompanyId { get; set; }
        public string CompanyName { get; set; }

        public List<SectionDto>? Sections { get; set; }
    }
}
