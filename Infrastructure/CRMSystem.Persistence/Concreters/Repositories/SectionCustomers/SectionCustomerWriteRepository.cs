using CRMSystem.Application.Absrtacts.Repositories.SectionCustomers;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Repositories.SectionCustomers
{
    public class SectionCustomerWriteRepository : WriteRepository<SectionCustomer>, ISectionCustomerWriteRepository
    {
        public SectionCustomerWriteRepository(CRMSystemDbContext CRMSystemDbContext) : base(CRMSystemDbContext)
        {
        }
    }
}
