using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.OrderOverheadDtos
{
    public class CreateOrderOverheadDto
    {
        public string Id { get; set; }
        public IFormFile InvoiceFile { get; set; }
    }
}
