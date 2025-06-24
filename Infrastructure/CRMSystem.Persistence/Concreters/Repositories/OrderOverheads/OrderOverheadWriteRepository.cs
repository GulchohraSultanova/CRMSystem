using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.OrderOverheads;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.OrderOverheads
{
    public class OrderOverheadWriteRepository : WriteRepository<OrderOverhead>, IOrderOverheadWriteRepository
    {
        public OrderOverheadWriteRepository(CRMSystemDbContext context) : base(context) { }
    }
}
