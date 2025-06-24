using CRMSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Customer
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Password { get; set; }
        public string? FinCode { get; set; }
        public string? JobId { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string>? SectionIds { get; set; }
    }
}
