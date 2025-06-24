using CRMSystem.Application.Dtos.Section;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Department
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; }
     
        public string CompanyId { get; set; }
    

    }
}
