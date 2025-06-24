using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Customer
{
    public class UpdateCustomerDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? JobId { get; set; }
        public string? Password { get; set; }
        public string? FinCode { get; set; }

    }
}
