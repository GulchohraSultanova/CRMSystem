using CRMSystem.Domain.Entities;
using CRMSystem.Application.Absrtacts.Repositories;

namespace CRMSystem.Application.Absrtacts.Repositories.Orders
{
    public interface IOrderWriteRepository : IWriteRepository<Order> { }
}
