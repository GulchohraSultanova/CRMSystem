using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.Companies;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Company;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyReadRepository _readRepository;
        private readonly ICompanyWriteRepository _writeRepository;
        private readonly IMediaService _fileService;
        private readonly IMapper _mapper;

        public CompanyService(
            ICompanyReadRepository readRepository,
            ICompanyWriteRepository writeRepository,
            IMediaService fileService,
            IMapper mapper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto)
        {
            var company = _mapper.Map<Company>(dto);
            company.CreatedDate = DateTime.Now;


            await _writeRepository.AddAsync(company);
            await _writeRepository.CommitAsync();

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<List<CompanyDto>> GetAllCompanyAsync()
        {
            var companies = await _readRepository.GetAllAsync(
                c => !c.IsDeleted,
        include: q => q.Include(c => c.Departments.Where(d => !d.IsDeleted))

            );

            return _mapper.Map<List<CompanyDto>>(companies);
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var company = await _readRepository.GetAsync(
                c => c.Id == guid && !c.IsDeleted,
                include: q => q.Include(c => c.Departments.Where(d=>!d.IsDeleted)
            ));

            if (company == null)
                throw new GlobalAppException("Şirkət tapılmadı!");

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<CompanyDto> UpdateCompanyAsync(UpdateCompanyDto dto)
        {
            if (!Guid.TryParse(dto.Id, out Guid companyId))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var company = await _readRepository.GetAsync(
                c => c.Id == companyId && !c.IsDeleted,
                include: q => q.Include(c => c.Departments.Where(d => !d.IsDeleted)
            ));

            if (company == null)
                throw new GlobalAppException("Şirkət tapılmadı!");

            company.Name = dto.Name ?? company.Name;
            company.LastUpdatedDate = DateTime.Now;

     

            await _writeRepository.UpdateAsync(company);
            await _writeRepository.CommitAsync();

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task DeleteCompanyAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var company = await _readRepository.GetAsync(c => c.Id == guid && !c.IsDeleted);
            if (company == null)
                throw new GlobalAppException("Şirkət tapılmadı!");

            company.IsDeleted = true;
            company.DeletedDate = DateTime.Now;

            await _writeRepository.UpdateAsync(company);
            await _writeRepository.CommitAsync();
        }
    }
}
