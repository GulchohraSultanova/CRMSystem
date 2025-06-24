using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    
    public class Order:BaseEntity
    {
        public List<OrderItem>? Items { get; set; }
        public List<OrderOverhead>? Overhead { get; set; }
        public Guid SectionId { get; set; }
        public string? FighterId { get; set; }
        public Admin? Fighter {  get; set; }

        public string AdminId { get; set; }
        public Section? Section { get; set; }
        public Admin? Admin { get; set; }
        public string? Description { get; set; }
        public bool EmployeeConfirm { get; set; } = false;
        public bool FighterConfirm { get; set; }=false;
        public bool EmployeeDelivery {  get; set; }=false;
        public DateTime OrderDeliveryTime { get; set; }
        public DateTime OrderLimitTime { get; set; }


    }
}
