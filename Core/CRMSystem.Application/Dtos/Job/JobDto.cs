using CRMSystem.Application.Dtos.Account;
using CRMSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Job
{
    public class JobDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

   
        public List<AdminInfoDto>? Customers { get; set; }
    }
}
