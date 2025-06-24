using CRMSystem.Application.Absrtacts.Repositories.AdminNotifications;
using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Repositories.AdminNotifications
{
    public class AdminNotificationWriteRepository : WriteRepository<AdminNotification>, IAdminNotificationWriteRepository
    {
        public AdminNotificationWriteRepository(CRMSystemDbContext CRMSystemDbContext) : base(CRMSystemDbContext)
        {
        }
    }
}
