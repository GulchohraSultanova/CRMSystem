using CRMSystem.Domain.Entities.Common;
using System;

namespace CRMSystem.Domain.Entities
{
    public class MonthlyCalculation : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public int Year { get; set; }
        public int Month { get; set; } // 1 - Yanvar, 2 - Fevral və s.

        public decimal InitialAmount { get; set; }           // Ayın əvvəlində təyin olunan
        public decimal TotalOrderAmount { get; set; }        // O ay verilən sifarişlərin məbləği
        public decimal TotalAmount => InitialAmount + TotalOrderAmount; // Computed in code, not db
    }
}
