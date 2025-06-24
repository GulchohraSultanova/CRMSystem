using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Vendors;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories.Vendors
{
    public class VendorWriteRepository : WriteRepository<Vendor>, IVendorWriteRepository
    {
        public VendorWriteRepository(CRMSystemDbContext context) : base(context) { }
    }
}
