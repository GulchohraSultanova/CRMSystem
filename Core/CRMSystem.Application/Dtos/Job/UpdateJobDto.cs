using CRMSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Job
{
    public class UpdateJobDto
    {
        public string Id { get; set; }
        public string Name { get; set; }


    }
}
