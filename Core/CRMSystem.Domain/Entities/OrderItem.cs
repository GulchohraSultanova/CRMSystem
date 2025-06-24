using CRMSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Domain.Entities
{
    public class OrderItem:BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public Guid? VendorId { get; set; }
        public Product? Product { get; set; }
        public Order? Order { get; set; }
        public Vendor? Vendor { get; set; }
        public decimal? Price { get; set; }
        public decimal RequiredQuantity { get; set; } 
        public decimal? SuppliedQuantity { get; set; }
        public DateTime OrderItemDeliveryTime { get; set; }
 
      

    }
}
