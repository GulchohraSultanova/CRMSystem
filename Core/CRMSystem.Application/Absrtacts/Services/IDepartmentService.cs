using CRMSystem.Application.Dtos.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);

        // Bütün şirkətləri gətir
        Task<List<DepartmentDto>> GetAllDepartmentAsync();
        Task<List<DepartmentDto>> GetDepartmentsByCompanyIdAsync(string companyId);


        // ID-yə əsasən bir şirkəti gətir
        Task<DepartmentDto> GetDepartmentByIdAsync(string id);

        // Güncəlləmə
        Task<DepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto dto);

        // Silmə (soft delete)
        Task DeleteDepartmentAsync(string id);
    }
}
