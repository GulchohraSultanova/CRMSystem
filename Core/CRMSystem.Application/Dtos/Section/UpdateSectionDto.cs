using CRMSystem.Application.Dtos.Job;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Section
{
    public class UpdateSectionDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }

        public string? DepartmentId { get; set; }


    }
}
