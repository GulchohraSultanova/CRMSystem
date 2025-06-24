using CRMSystem.Domain.Entities;
using CRMSystem.Application.Absrtacts.Repositories;

namespace CRMSystem.Application.Absrtacts.Repositories.Orders
{
    public interface IOrderReadRepository : IReadRepository<Order> { }
}
