using CRMSystem.Application.Dtos.OrderItem;
using CRMSystem.Application.Dtos.OrderItemCustomer;
using CRMSystem.Application.Dtos.OrderOverheadDtos;
using CRMSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.OrderCustomer
{
    public class CreateOrderDto
    {
        public string SectionId { get; set; }
        public string Description { get; set; }

        public string OrderLimitTime { get; set; }
      
        public List<CreateOrderItemDto> Items { get; set; }


    }
}
