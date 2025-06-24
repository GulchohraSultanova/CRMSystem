using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class Vendor:BaseEntity
    {
        public string Name { get; set; }
        public string? VendorImage { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
   
    }
}
