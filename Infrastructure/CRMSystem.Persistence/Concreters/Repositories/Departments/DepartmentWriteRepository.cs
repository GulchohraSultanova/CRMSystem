using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Departments;
using CRMSystem.Application.Absrtacts.Repositories.PendingCategories;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Departments
{
    public class DepartmentWriteRepository : WriteRepository<Department>, IDepartmentWriteRepository
    {
        public DepartmentWriteRepository(CRMSystemDbContext context) : base(context) { }
    }
}
