using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Statistic
{
    public class MonthlyFighterOrderStatusDto
    {
        public string Month { get; set; }
        public int Completed { get; set; }
        public int Incomplete { get; set; }
    }

}
