using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Jobs;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Jobs
{
    public class JobWriteRepository : WriteRepository<Job>, IJobWriteRepository
    {
        public JobWriteRepository(CRMSystemDbContext context) : base(context) { }
    }
}
