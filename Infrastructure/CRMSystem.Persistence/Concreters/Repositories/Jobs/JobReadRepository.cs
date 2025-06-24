using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Jobs;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Jobs
{
    public class JobReadRepository : ReadRepository<Job>, IJobReadRepository
    {
        public JobReadRepository(CRMSystemDbContext context) : base(context) { }
    }
}
