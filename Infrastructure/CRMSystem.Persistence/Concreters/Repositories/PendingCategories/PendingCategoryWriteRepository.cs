using CRMSystem.Application.Absrtacts.Repositories.PendingCategories;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Concreters.Repositories.Categories;
using CRMSystem.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Repositories.PendingCategories
{
    public class PendingCategoryWriteRepository : WriteRepository<PendingCategory>, IPendingCategoryWriteRepository
    {
        public PendingCategoryWriteRepository(CRMSystemDbContext CRMSystemDbContext) : base(CRMSystemDbContext)
        {
        }
    }
}
