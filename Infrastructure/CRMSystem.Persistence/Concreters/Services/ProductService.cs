using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.Products;
using CRMSystem.Application.Absrtacts.Repositories.PendingProducts;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Product;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _read;
        private readonly IProductWriteRepository _write;
        private readonly IPendingProductReadRepository _pendRead;
        private readonly IPendingProductWriteRepository _pendWrite;
        private readonly IAdminNotificationService _notificationService;
        private readonly IMapper _mapper;

        public ProductService(
            IProductReadRepository read,
            IProductWriteRepository write,
            IPendingProductReadRepository pendRead,
            IPendingProductWriteRepository pendWrite,
            IAdminNotificationService notificationService,
            IMapper mapper)
        {
            _read = read;
            _write = write;
            _pendRead = pendRead;
            _pendWrite = pendWrite;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        // ─── READ (approved + created + not deleted) ───────────────────────────────

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var items = await _read.GetAllAsync(
                p => p.IsCreated && !p.IsDeleted && p.IsUpdated,
                include: q => q.Include(x => x.Category),
                orderBy: q => q.OrderBy(x => x.Name)
            );
            return _mapper.Map<List<ProductDto>>(items);
        }

        public async Task<ProductDto> GetByIdAsync(string id)
        {
            var p = await _read.GetAsync(
                p => p.Id.ToString() == id && p.IsCreated && !p.IsDeleted && p.IsUpdated,
                include: q => q.Include(x => x.Category)
            );
            if (p == null)
                throw new GlobalAppException("Product not found!");
            return _mapper.Map<ProductDto>(p);
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(string categoryId)
        {
            if (!Guid.TryParse(categoryId, out var catGuid))
                throw new GlobalAppException("Category ID formatı düzgün deyil!");

            var items = await _read.GetAllAsync(
                p => p.CategoryId == catGuid && p.IsCreated && p.IsUpdated && !p.IsDeleted,
                include: q => q.Include(x => x.Category),
                orderBy: q => q.OrderBy(x => x.Name)
            );
            return _mapper.Map<List<ProductDto>>(items);
        }

        // ─── PENDING READ ───────────────────────────────────────────────────────────

        public async Task<List<ProductDto>> GetPendingAddsAsync()
        {
            var list = await _read.GetAllAsync(p => !p.IsCreated && !p.IsDeleted);
            return _mapper.Map<List<ProductDto>>(list);
        }

        public async Task<List<ProductDto>> GetPendingDeletesAsync()
        {
            var list = await _read.GetAllAsync(p => p.Deleted && !p.IsDeleted);
            return _mapper.Map<List<ProductDto>>(list);
        }

        public async Task<List<PendingProductDetailsDto>> GetPendingUpdatesAsync()
        {
            var pendings = await _pendRead.GetAllAsync(
                p => !p.IsDeleted,
                include: q => q.Include(x => x.OriginalProduct)
            );
            return pendings
                .Select(p => new PendingProductDetailsDto
                {
                    Id = p.Id.ToString(),
                    OldName = p.OriginalProduct!.Name,
                    NewName = p.NewName,
                    OldMeasure = p.OriginalProduct.Measure,
                    NewMeasure = p.NewMeasure,
                    IsUpdated = p.OriginalProduct.IsUpdated
                })
                .ToList();
        }

        // ─── REQUEST (all authenticated) ────────────────────────────────────────────

        public async Task RequestCreateAsync(string role, CreateProductDto dto)
        {
            var prod = _mapper.Map<Product>(dto);
            prod.IsCreated = (role == "SuperAdmin");
            prod.Deleted = false;
            prod.IsUpdated = true;
            prod.CreatedDate = DateTime.Now;
            prod.LastUpdatedDate = DateTime.Now;

            await _write.AddAsync(prod);
            await _write.CommitAsync();

            // Fire “create” notification
            if (role != "SuperAdmin")
            {
                await _notificationService.CreateAsync(
              type: "create",
              categoryId: null,
              orderId: null,  
              productId: prod.Id.ToString()
          );
            }
        }

        public async Task RequestDeleteAsync(string role, string id)
        {
            var prod = await _read.GetAsync(p => p.Id.ToString() == id);
            if (prod == null)
                throw new GlobalAppException("Product not found!");

            prod.Deleted = true;
            if (role == "SuperAdmin")
                prod.IsDeleted = true;
            prod.LastUpdatedDate = DateTime.Now;

            await _write.UpdateAsync(prod);
            await _write.CommitAsync();

            // Fire “delete” notification
         if(role != "SuperAdmin")
            {
                await _notificationService.CreateAsync(
             type: "delete",
             orderId : null,
             categoryId: null,
             productId: prod.Id.ToString()
         );
            }
        }

        public async Task RequestUpdateAsync(string role, UpdatePendingProductDto dto)
        {
            var orig = await _read.GetAsync(p => p.Id.ToString() == dto.Id);
            if (orig == null)
                throw new GlobalAppException("Product not found!");

            if (role == "SuperAdmin")
            {
                orig.Name = dto.NewName ?? orig.Name;
                orig.Measure = dto.NewMeasure ?? orig.Measure;
                if (!string.IsNullOrWhiteSpace(dto.CategoryId)
                    && Guid.TryParse(dto.CategoryId, out var catGuid))
                {
                    orig.CategoryId = catGuid;
                }
                orig.IsUpdated = true;
                orig.LastUpdatedDate = DateTime.Now;

                await _write.UpdateAsync(orig);
                await _write.CommitAsync();

                // Fire “update” notification
        
                
            }
            else
            {
                var pend = new PendingProduct
                {
                    OriginalProductId = orig.Id,
                    NewName = dto.NewName,
                    NewMeasure = dto.NewMeasure,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                };
                await _pendWrite.AddAsync(pend);
                await _pendWrite.CommitAsync();

                // Fire “update” notification (pending)
                await _notificationService.CreateAsync(
                    orderId:null,
                    type: "update",
                    categoryId: null,
                    productId: orig.Id.ToString()
                );
            }
        }

        // ─── APPROVE / REJECT CREATE ────────────────────────────────────────────────

        public async Task ApproveCreateAsync(string id)
        {
            var prod = await _read.GetAsync(
                p => p.Id.ToString() == id && !p.IsCreated && !p.IsDeleted
            );
            if (prod == null)
                throw new GlobalAppException("Pending creation not found!");

            prod.IsCreated = true;
            prod.LastUpdatedDate = DateTime.Now;

            await _write.UpdateAsync(prod);
            await _write.CommitAsync();
        }

        public async Task RejectCreateAsync(string id)
        {
            var prod = await _read.GetAsync(
                p => p.Id.ToString() == id && !p.IsCreated && !p.IsDeleted
            );
            if (prod == null)
                throw new GlobalAppException("Pending creation not found!");

            await _write.HardDeleteAsync(prod);
            await _write.CommitAsync();
        }

        // ─── APPROVE / REJECT DELETE ────────────────────────────────────────────────

        public async Task ApproveDeleteAsync(string id)
        {
            var prod = await _read.GetAsync(
                p => p.Id.ToString() == id && p.Deleted && !p.IsDeleted
            );
            if (prod == null)
                throw new GlobalAppException("Pending deletion not found!");

            prod.IsDeleted = true;
            prod.DeletedDate = DateTime.Now;
            prod.LastUpdatedDate = DateTime.Now;

            await _write.UpdateAsync(prod);
            await _write.CommitAsync();
        }

        public async Task RejectDeleteAsync(string id)
        {
            var prod = await _read.GetAsync(
                p => p.Id.ToString() == id && p.Deleted && !p.IsDeleted
            );
            if (prod == null)
                throw new GlobalAppException("Pending deletion not found!");

            prod.Deleted = false;
            prod.LastUpdatedDate = DateTime.Now;

            await _write.UpdateAsync(prod);
            await _write.CommitAsync();
        }

        // ─── APPROVE / REJECT UPDATE ────────────────────────────────────────────────

        public async Task ApproveUpdateAsync(string pendingId)
        {
            var pend = await _pendRead.GetAsync(
                p => p.Id.ToString() == pendingId && !p.IsDeleted,
                include: q => q.Include(x => x.OriginalProduct)
            );
            if (pend == null)
                throw new GlobalAppException("Pending update not found!");

            var prod = pend.OriginalProduct!;
            prod.Name = pend.NewName!;
            prod.Measure = pend.NewMeasure!;
            prod.IsUpdated = true;
            prod.LastUpdatedDate = DateTime.Now;

            await _write.UpdateAsync(prod);
            await _pendWrite.HardDeleteAsync(pend);
            await _pendWrite.CommitAsync();
        }

        public async Task RejectUpdateAsync(string pendingId)
        {
            var pend = await _pendRead.GetAsync(p => p.Id.ToString() == pendingId && !p.IsDeleted);
            if (pend == null)
                throw new GlobalAppException("Pending update not found!");

            await _pendWrite.HardDeleteAsync(pend);
            await _pendWrite.CommitAsync();
        }
    }
}
