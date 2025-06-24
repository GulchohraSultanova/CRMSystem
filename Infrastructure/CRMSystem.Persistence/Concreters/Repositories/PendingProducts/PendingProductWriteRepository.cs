using CRMSystem.Application.Absrtacts.Repositories.PendingCategories;
using CRMSystem.Application.Absrtacts.Repositories.PendingProducts;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Repositories.PendingProducts
{
    public class PendingProductWriteRepository : WriteRepository<PendingProduct>, IPendingProductWriteRepository

    {
        public PendingProductWriteRepository(CRMSystemDbContext CRMSystemDbContext) : base(CRMSystemDbContext)
        {
        }


    }
}
