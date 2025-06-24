using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Product
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsCreated { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public bool IsUpdated { get; set; } = false;
        public string CategoryId { get; set; }

        public string Measure { get; set; } 
    }
}
