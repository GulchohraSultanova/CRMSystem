using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Statistic
{
    public class MonthlyOrderStatusDto
    {
        public string Month { get; set; }
        public int CompletedCount { get; set; }
        public int CanceledCount { get; set; }
        public int PendingCount { get; set; }
    }

}
