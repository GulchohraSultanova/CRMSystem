using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Orders;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Orders
{
    public class OrderReadRepository : ReadRepository<Order>, IOrderReadRepository


    {
        public OrderReadRepository(CRMSystemDbContext context) : base(context) { }
    }
}
