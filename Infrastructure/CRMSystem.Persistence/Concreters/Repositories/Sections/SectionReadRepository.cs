using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Sections;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Sections
{
    public class SectionReadRepository : ReadRepository<Section>, ISectionReadRepository
    {
        public SectionReadRepository(CRMSystemDbContext context) : base(context) { }
    }
}
