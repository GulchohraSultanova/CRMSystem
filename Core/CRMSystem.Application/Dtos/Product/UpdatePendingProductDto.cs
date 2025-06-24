using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Product
{
    public class UpdatePendingProductDto
    {
        public string Id { get; set; }
        public string NewName { get; set; }

        public string CategoryId { get; set; }

        public string NewMeasure { get; set; } 
    }
}
