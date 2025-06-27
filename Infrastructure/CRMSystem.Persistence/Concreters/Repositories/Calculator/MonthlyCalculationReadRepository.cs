using CRMSystem.Application.Absrtacts.Repositories.Calculator;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Repositories.Calculator
{
    public class MonthlyCalculationReadRepository : ReadRepository<MonthlyCalculation>, IMonthlyCalculationReadRepository
    {
        public MonthlyCalculationReadRepository(CRMSystemDbContext CRMSystemDbContext) : base(CRMSystemDbContext)
        {
        }
    }
}
