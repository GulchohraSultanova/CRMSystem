using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.OrderItems;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.OrderItems
{
    public class OrderItemReadRepository : ReadRepository<OrderItem>, IOrderItemReadRepository
    {
        public OrderItemReadRepository(CRMSystemDbContext context) : base(context) { }
    }
}
