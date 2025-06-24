using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Product
{
    public class PendingProductDetailsDto
    {
        public string Id { get; set; }
        public string? OldName { get; set; }
        public string? OldMeasure {  get; set; }
        public string? NewName { get; set; }
        public string? NewMeasure {  get; set; }


        public bool IsUpdated { get; set; }
    }
}
