using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.Sections;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Section;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionReadRepository _readRepository;
        private readonly ISectionWriteRepository _writeRepository;
        private readonly IMediaService _fileService;
        private readonly IMapper _mapper;

        public SectionService(
            ISectionReadRepository readRepository,
            ISectionWriteRepository writeRepository,
            IMediaService fileService,
            IMapper mapper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<SectionDto> CreateSectionAsync(CreateSectionDto dto)
        {
            var section = _mapper.Map<Section>(dto);

     

            section.CreatedDate = DateTime.UtcNow;
            section.LastUpdatedDate = DateTime.UtcNow;
            section.IsDeleted = false;

            await _writeRepository.AddAsync(section);
            await _writeRepository.CommitAsync();

            // Yenidən DTO-ya çevir

            return _mapper.Map<SectionDto>(section);
        }

        public async Task<List<SectionDto>> GetAllSectionsAsync()
        {
            var sections = await _readRepository.GetAllAsync(
                s => !s.IsDeleted,
                include: q => q
                    .Include(s => s.CustomerList)
                        .ThenInclude(sc => sc.Admin)
            );

            return _mapper.Map<List<SectionDto>>(sections);
        }

        public async Task<SectionDto> GetSectionByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var section = await _readRepository.GetAsync(
                s => s.Id == guid && !s.IsDeleted,
                include: q => q
                    .Include(s => s.CustomerList)
                        .ThenInclude(sc => sc.Admin)
            );

            if (section == null)
                throw new GlobalAppException("Bölmə tapılmadı!");

            return _mapper.Map<SectionDto>(section);
        }

        public async Task<List<SectionDto>> GetSectionsByDepartmentIdAsync(string departmentId)
        {
            if (!Guid.TryParse(departmentId, out var depGuid))
                throw new GlobalAppException("Department ID formatı düzgün deyil!");

            var sections = await _readRepository.GetAllAsync(
                s => s.DepartmentId == depGuid && !s.IsDeleted,
                include: q => q
                    .Include(s => s.CustomerList)
                        .ThenInclude(sc => sc.Admin)
            );

            return _mapper.Map<List<SectionDto>>(sections);
        }

        public async Task<SectionDto> UpdateSectionAsync(UpdateSectionDto dto)
        {
            if (!Guid.TryParse(dto.Id, out var guid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var section = await _readRepository.GetAsync(s => s.Id == guid && !s.IsDeleted);
            if (section == null)
                throw new GlobalAppException("Bölmə tapılmadı!");

            // Sahələri güncəllə
            section.Name = dto.Name ?? section.Name;
            section.LastUpdatedDate = DateTime.UtcNow;

            // Department dəyişibsə
            if (!string.IsNullOrWhiteSpace(dto.DepartmentId)
                && Guid.TryParse(dto.DepartmentId, out var depGuid2))
            {
                section.DepartmentId = depGuid2;
            }

            // Yeni şəkil gəlibsə
  

            await _writeRepository.UpdateAsync(section);
            await _writeRepository.CommitAsync();

            return _mapper.Map<SectionDto>(section);
        }

        public async Task DeleteSectionAsync(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var section = await _readRepository.GetAsync(s => s.Id == guid && !s.IsDeleted);
            if (section == null)
                throw new GlobalAppException("Bölmə tapılmadı!");

            section.IsDeleted = true;
            section.DeletedDate = DateTime.UtcNow;

            await _writeRepository.UpdateAsync(section);
            await _writeRepository.CommitAsync();
        }
    }
}
