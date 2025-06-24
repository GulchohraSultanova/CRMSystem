using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.Vendors;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Vendor;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorReadRepository _vendorRead;
        private readonly IVendorWriteRepository _vendorWrite;
        private readonly IMapper _mapper;

        public VendorService(
            IVendorReadRepository vendorRead,
            IVendorWriteRepository vendorWrite,
            IMapper mapper)
        {
            _vendorRead = vendorRead;
            _vendorWrite = vendorWrite;
            _mapper = mapper;
        }

        public async Task<VendorDto> CreateVendorAsync(CreateVendorDto dto)
        {
            var entity = new Vendor
            {
                Name = dto.Name,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };
            await _vendorWrite.AddAsync(entity);
            await _vendorWrite.CommitAsync();

            var vendorWithItems = await _vendorRead.GetAsync(
                v => v.Id == entity.Id,
                include: q => q.Include(v => v.OrderItems)
            );
            return _mapper.Map<VendorDto>(vendorWithItems);
        }

        public async Task<List<VendorDto>> GetAllVendorsAsync()
        {
            var list = await _vendorRead.GetAllAsync(
                include: q => q.Include(v => v.OrderItems),
                orderBy: q => q.OrderBy(v => v.Name)
            );
            return _mapper.Map<List<VendorDto>>(list);
        }

        public async Task<VendorDto> GetVendorByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                throw new GlobalAppException("ID formatı düzgün deyil.");

            var entity = await _vendorRead.GetAsync(
                v => v.Id == guid,
                include: q => q.Include(v => v.OrderItems)
            );
            if (entity == null)
                throw new GlobalAppException("Vendor tapılmadı.");
            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<VendorDto> UpdateVendorAsync(UpdateVendorDto dto)
        {
            if (!Guid.TryParse(dto.Id, out var guid))
                throw new GlobalAppException("ID formatı düzgün deyil.");

            var entity = await _vendorRead.GetAsync(v => v.Id == guid);
            if (entity == null)
                throw new GlobalAppException("Vendor tapılmadı.");

            entity.Name = dto.Name ?? entity.Name;
            entity.LastUpdatedDate = DateTime.UtcNow;

            await _vendorWrite.UpdateAsync(entity);
            await _vendorWrite.CommitAsync();

            return _mapper.Map<VendorDto>(entity);
        }

        public async Task DeleteVendorAsync(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                throw new GlobalAppException("ID formatı düzgün deyil.");

            var entity = await _vendorRead.GetAsync(v => v.Id == guid);
            if (entity == null)
                throw new GlobalAppException("Vendor tapılmadı.");

            await _vendorWrite.HardDeleteAsync(entity);
            await _vendorWrite.CommitAsync();
        }
    }
}
