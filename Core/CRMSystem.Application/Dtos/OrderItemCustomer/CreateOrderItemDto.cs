using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.OrderItemCustomer
{
    public class CreateOrderItemDto
    {

        public string ProductId { get; set; }

        public decimal RequiredQuantity { get; set; }



    }
}
