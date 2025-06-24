using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Departments;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Departments
{
    public class DepartmentReadRepository : ReadRepository<Department>, IDepartmentReadRepository
    {
        public DepartmentReadRepository(CRMSystemDbContext context) : base(context) { }
    }
}
