using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.OrderItem
{
    public class OrderItemDto
    {

        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal RequiredQuantity { get; set; }
        public decimal? SuppliedQuantity { get; set; }
        public decimal? Price { get; set; }
        public string VendorName { get; set; }
        public string OrderItemDeliveryTime { get; set; }
    }
}
