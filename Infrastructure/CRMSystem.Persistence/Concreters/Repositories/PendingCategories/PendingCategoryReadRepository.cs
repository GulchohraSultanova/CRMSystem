using CRMSystem.Application.Absrtacts.Repositories.PendingCategories;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Repositories.PendingCategories
{
    public class PendingCategoryReadRepository : ReadRepository<PendingCategory>, IPendingCategoryReadRepository
    {
        public PendingCategoryReadRepository(CRMSystemDbContext CRMSystemDbContext) : base(CRMSystemDbContext)
        {
        }
    }
}
