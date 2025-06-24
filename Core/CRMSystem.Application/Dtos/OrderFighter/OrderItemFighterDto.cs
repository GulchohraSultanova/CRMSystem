using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.OrderFighter
{
    public class OrderItemFighterDto
    {
        public string OrderItemId { get; set; }
        public decimal Price { get; set; }
        public decimal SuppliedQuantity { get; set; }
        public string VendorId { get; set; }
    }
}
