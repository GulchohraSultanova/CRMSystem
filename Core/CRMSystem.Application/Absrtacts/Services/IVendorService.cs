using CRMSystem.Application.Dtos.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IVendorService
    {
        Task<VendorDto> CreateVendorAsync(CreateVendorDto dto);
        Task<List<VendorDto>> GetAllVendorsAsync();
        Task<VendorDto> GetVendorByIdAsync(string id);
  
        Task<VendorDto> UpdateVendorAsync(UpdateVendorDto dto);
        Task DeleteVendorAsync(string id);
    }
}
