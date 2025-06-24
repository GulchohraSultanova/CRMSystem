using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Vendors;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Vendors
{
    public class VendorReadRepository : ReadRepository<Vendor>, IVendorReadRepository
    {
        public VendorReadRepository(CRMSystemDbContext context) : base(context) { }
    }
}
