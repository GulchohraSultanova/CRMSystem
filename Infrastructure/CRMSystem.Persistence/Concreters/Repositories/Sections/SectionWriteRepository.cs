using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Sections;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Sections
{
    public class SectionWriteRepository : WriteRepository<Section>, ISectionWriteRepository
    {
        public SectionWriteRepository(CRMSystemDbContext context) : base(context) { }
    }
}
