using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }

        public string CategoryId { get; set; }

        public string Measure { get; set; } = "kq";
    }
}
