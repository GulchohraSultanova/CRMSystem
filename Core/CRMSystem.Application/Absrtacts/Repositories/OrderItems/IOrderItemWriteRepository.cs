using CRMSystem.Domain.Entities;
using CRMSystem.Application.Absrtacts.Repositories;

namespace CRMSystem.Application.Absrtacts.Repositories.OrderItems
{
    public interface IOrderItemWriteRepository : IWriteRepository<OrderItem> { }
}
