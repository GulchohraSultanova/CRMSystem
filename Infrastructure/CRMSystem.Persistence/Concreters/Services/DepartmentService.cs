using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.Departments;
using CRMSystem.Application.Absrtacts.Repositories.PendingCategories;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Department;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentReadRepository _readRepository;

        public DepartmentService(IDepartmentReadRepository readRepository, IDepartmentWriteRepository writeRepository, IMapper mapper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        private readonly IDepartmentWriteRepository _writeRepository;
   
        private readonly IMapper _mapper;


        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            var department = _mapper.Map<Department>(dto);
            department.CreatedDate = DateTime.Now;

     

            await _writeRepository.AddAsync(department);
            await _writeRepository.CommitAsync();

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<List<DepartmentDto>> GetAllDepartmentAsync()
        {
            var departments = await _readRepository.GetAllAsync(
                d => !d.IsDeleted,
                include: q => q.Include(d => d.Sections.Where(d => !d.IsDeleted))
            );

            return _mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var department = await _readRepository.GetAsync(
                d => d.Id == parsedId && !d.IsDeleted,
                include: q => q.Include(d => d.Sections.Where(d => !d.IsDeleted))
            );

            if (department == null)
                throw new GlobalAppException("Şöbə tapılmadı!");

            return _mapper.Map<DepartmentDto>(department);
        }
        public async Task<List<DepartmentDto>> GetDepartmentsByCompanyIdAsync(string companyId)
        {
            if (!Guid.TryParse(companyId, out Guid parsedCompanyId))
                throw new GlobalAppException("Company ID formatı düzgün deyil!");

            var departments = await _readRepository.GetAllAsync(
                d => d.CompanyId == parsedCompanyId && !d.IsDeleted,
                include: q => q.Include(d => d.Sections.Where(d => !d.IsDeleted))
            );

            return _mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto dto)
        {
            if (!Guid.TryParse(dto.Id, out Guid departmentId))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var department = await _readRepository.GetAsync(d => d.Id == departmentId && !d.IsDeleted);
            if (department == null)
                throw new GlobalAppException("Şöbə tapılmadı!");

            // AutoMapper mapping (şəkil və nested navigationlar çıxılmaqla)
   

            department.Name = dto.Name ?? department.Name;

            // 🔄 CompanyId güncəllənməsi (null deyilsə və format düzgünsə)
            if (!string.IsNullOrEmpty(dto.CompanyId) && Guid.TryParse(dto.CompanyId, out Guid companyGuid))
                department.CompanyId = companyGuid;

            department.LastUpdatedDate = DateTime.Now;

            // 📂 Yeni şəkil varsa yüklə

       
            await _writeRepository.UpdateAsync(department);
            await _writeRepository.CommitAsync();

            return _mapper.Map<DepartmentDto>(department);
        }


        public async Task DeleteDepartmentAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid departmentId))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var department = await _readRepository.GetAsync(d => d.Id == departmentId && !d.IsDeleted);
            if (department == null)
                throw new GlobalAppException("Şöbə tapılmadı!");

            department.IsDeleted = true;
            department.DeletedDate = DateTime.Now;

            await _writeRepository.UpdateAsync(department);
            await _writeRepository.CommitAsync();
        }
    }
}
