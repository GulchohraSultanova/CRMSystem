using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class Admin:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? FinCode { get; set; }
        public Guid? JobId { get; set; }
        public Job? Job { get; set; }
        public List<SectionCustomer>? Sections { get; set; }



    }
}
