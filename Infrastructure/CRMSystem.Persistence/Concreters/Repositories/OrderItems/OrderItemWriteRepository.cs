using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.OrderItems;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.OrderItems
{
    public class OrderItemWriteRepository : WriteRepository<OrderItem>, IOrderItemWriteRepository
    {
        public OrderItemWriteRepository(CRMSystemDbContext context) : base(context) { }
    }
}
