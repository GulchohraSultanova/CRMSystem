using CRMSystem.Application.Dtos.Company;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface ICompanyService
    {
        // Yeni şirkət əlavə et
        Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto);

        // Bütün şirkətləri gətir
        Task<List<CompanyDto>> GetAllCompanyAsync();

        // ID-yə əsasən bir şirkəti gətir
        Task<CompanyDto> GetCompanyByIdAsync(string id);

        // Güncəlləmə
        Task<CompanyDto> UpdateCompanyAsync(UpdateCompanyDto dto);

        // Silmə (soft delete)
        Task DeleteCompanyAsync(string id);
    }
}
