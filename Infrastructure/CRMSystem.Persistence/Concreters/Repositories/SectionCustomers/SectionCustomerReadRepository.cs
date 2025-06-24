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
    public class SectionCustomerReadRepository : ReadRepository<SectionCustomer>, ISectionCustomerReadRepository
    {
        public SectionCustomerReadRepository(CRMSystemDbContext CRMSystemDbContext) : base(CRMSystemDbContext)
        {
        }
    }
}
