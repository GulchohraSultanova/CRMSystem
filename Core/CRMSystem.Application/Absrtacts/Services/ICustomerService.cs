using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.Dtos.Company;
using CRMSystem.Application.Dtos.Customer;
using CRMSystem.Application.Dtos.Department;
using CRMSystem.Application.Dtos.Section;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto);
        Task<List<SectionDto>> GetSectionsByCustomerAsync(string customerId, string departmentId);
        Task<List<CompanyDto>> GetCompaniesByCustomerAsync(string customerId);
        Task<List<DepartmentDto>> GetDepartmentsByCustomerAsync(string customerId, string companyId);
        Task ChangePasswordAsync(string customerId, ChangePasswordDto dto);
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerByIdAsync(string id);
        Task<CustomerDto> UpdateCustomerAsync(UpdateCustomerDto dto);
        Task DeleteCustomerAsync(string id);
        Task RemoveSectionFromCustomerAsync(string customerId, string sectionId);

        Task AssignSectionsToCustomerAsync(string customerId, List<string> sectionIds);
    }
}
