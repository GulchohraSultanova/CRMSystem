using CRMSystem.Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Category
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool IsCreated { get; set; }
        public bool Deleted { get; set; } 
        public bool IsUpdated { get; set; } 
        public List<ProductDto>? Products { get; set; }
    }
}
