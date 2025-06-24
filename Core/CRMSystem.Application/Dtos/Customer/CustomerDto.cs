using CRMSystem.Application.Dtos.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Customer
{
    public class CustomerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<SectionDto> Sections { get; set; }
        public string Password { get; set; }
        public string? FinCode { get; set; }
        public string JobId { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
