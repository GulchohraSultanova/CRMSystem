using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.Categories;
using CRMSystem.Application.Absrtacts.Repositories.PendingCategories;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Category;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryReadRepository _categoryRead;
        private readonly ICategoryWriteRepository _categoryWrite;
        private readonly IPendingCategoryReadRepository _pendingRead;
        private readonly IPendingCategoryWriteRepository _pendingWrite;
        private readonly IAdminNotificationService _notificationService;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryReadRepository categoryRead,
            ICategoryWriteRepository categoryWrite,
            IPendingCategoryReadRepository pendingRead,
            IPendingCategoryWriteRepository pendingWrite,
            IAdminNotificationService notificationService,
            IMapper mapper)
        {
            _categoryRead = categoryRead;
            _categoryWrite = categoryWrite;
            _pendingRead = pendingRead;
            _pendingWrite = pendingWrite;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        // ─── READ ────────────────────────────────────────────────────────────────────

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRead.GetAllAsync(
                c => !c.IsDeleted && c.IsUpdated && c.IsCreated,
                include: q => q.Include(x => x.Products.Where(p => !p.IsDeleted))
            );
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string id)
        {
            var category = await _categoryRead.GetAsync(
                c => c.Id.ToString() == id && !c.IsDeleted && c.IsUpdated && c.IsCreated,
                include: q => q.Include(x => x.Products.Where(p => !p.IsDeleted))
            );
            if (category == null)
                throw new GlobalAppException("Category not found!");
            return _mapper.Map<CategoryDto>(category);
        }

        // ─── PENDING READ ────────────────────────────────────────────────────────────

        public async Task<List<CategoryDto>> GetAllPendingAddsAsync()
        {
            var list = await _categoryRead.GetAllAsync(c => !c.IsCreated && !c.IsDeleted);
            return _mapper.Map<List<CategoryDto>>(list);
        }

        public async Task<List<CategoryDto>> GetAllPendingDeletesAsync()
        {
            var list = await _categoryRead.GetAllAsync(c => c.Deleted && !c.IsDeleted);
            return _mapper.Map<List<CategoryDto>>(list);
        }

        public async Task<List<PendingCategoryDetailsDto>> GetAllPendingUpdatesAsync()
        {
            var pendings = await _pendingRead.GetAllAsync(
                p => !p.IsDeleted,
                include: q => q.Include(x => x.OriginalCategory)
            );

            return pendings
                .Select(p => new PendingCategoryDetailsDto
                {
                    Id = p.Id.ToString(),
                    OldName = p.OriginalCategory?.Name,
                    NewName = p.NewName,
                    IsUpdated = p.OriginalCategory?.IsUpdated ?? false
                })
                .ToList();
        }

        public async Task<PendingCategoryDetailsDto> GetPendingByIdAsync(string id)
        {
            var pend = await _pendingRead.GetAsync(
                p => p.Id.ToString() == id && !p.IsDeleted,
                include: q => q.Include(x => x.OriginalCategory)
            );
            if (pend == null)
                throw new GlobalAppException("Pending not found!");

            return new PendingCategoryDetailsDto
            {
                Id = pend.Id.ToString(),
                OldName = pend.OriginalCategory?.Name,
                NewName = pend.NewName,
                IsUpdated = pend.OriginalCategory?.IsUpdated ?? false
            };
        }

        // ─── REQUEST (by role) ────────────────────────────────────────────────────────

        public async Task RequestCreateCategoryAsync(string role, CreateCategoryDto dto)
        {
            var cat = new Category
            {
                Name = dto.Name,
                IsCreated = (role == "SuperAdmin"),
                Deleted = false,
                IsUpdated = true,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            };

            await _categoryWrite.AddAsync(cat);
            await _categoryWrite.CommitAsync();

            // Fire “create” notification
            if (role != "SuperAdmin")
            {
                await _notificationService.CreateAsync(
         type: "create",
         categoryId: cat.Id.ToString(),
         orderId:null,
         productId: null
     );
            }
        }

        public async Task RequestDeleteCategoryAsync(string role, string id)
        {
            var cat = await _categoryRead.GetAsync(c => c.Id.ToString() == id);
            if (cat == null)
                throw new GlobalAppException("Category not found!");

            cat.Deleted = true;
            if (role == "SuperAdmin")
                cat.IsDeleted = true;
            cat.LastUpdatedDate = DateTime.Now;

            await _categoryWrite.UpdateAsync(cat);
            await _categoryWrite.CommitAsync();

            // Fire “delete” notification
            if (role != "SuperAdmin")
            {
                await _notificationService.CreateAsync(
                    orderId: null,
       type: "delete",
       categoryId: cat.Id.ToString(),
       productId: null
   );
            }
        }

        public async Task RequestUpdateCategoryAsync(string role, UpdatePendingCategoryDto dto)
        {
            var orig = await _categoryRead.GetAsync(c => c.Id.ToString() == dto.Id);
            if (orig == null)
                throw new GlobalAppException("Category not found!");

            if (role == "SuperAdmin")
            {
                orig.Name = dto.NewName ?? orig.Name;
                orig.IsUpdated = true;
                orig.LastUpdatedDate = DateTime.Now;

                await _categoryWrite.UpdateAsync(orig);
                await _categoryWrite.CommitAsync();

                // Fire “update” notification
         
            }
            else
            {
                var pend = new PendingCategory
                {
                    OriginalCategoryId = orig.Id,
                    NewName = dto.NewName,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                };
                await _pendingWrite.AddAsync(pend);
                await _pendingWrite.CommitAsync();

                // Fire “update” notification (pending)
                await _notificationService.CreateAsync(
                    orderId: null,
                    type: "update",
                    categoryId: orig.Id.ToString(),
                    productId: null
                );
            }
        }

        // ─── APPROVE / REJECT CREATE ─────────────────────────────────────────────────

        public async Task ApproveCategoryCreateAsync(string categoryId)
        {
            var cat = await _categoryRead.GetAsync(
                c => c.Id.ToString() == categoryId
                  && !c.IsCreated
                  && !c.IsDeleted
            );
            if (cat == null)
                throw new GlobalAppException("Pending creation not found!");

            cat.IsCreated = true;
            cat.LastUpdatedDate = DateTime.Now;

            await _categoryWrite.UpdateAsync(cat);
            await _categoryWrite.CommitAsync();

            // (Optional) do not fire another notification here
        }

        public async Task RejectCategoryCreateAsync(string categoryId)
        {
            var cat = await _categoryRead.GetAsync(
                c => c.Id.ToString() == categoryId
                  && !c.IsCreated
                  && !c.IsDeleted
            );
            if (cat == null)
                throw new GlobalAppException("Pending creation not found!");

            await _categoryWrite.HardDeleteAsync(cat);
            await _categoryWrite.CommitAsync();
        }

        // ─── APPROVE / REJECT DELETE ─────────────────────────────────────────────────

        public async Task ApproveCategoryDeleteAsync(string categoryId)
        {
            var cat = await _categoryRead.GetAsync(
                c => c.Id.ToString() == categoryId
                  && c.Deleted
                  && !c.IsDeleted
            );
            if (cat == null)
                throw new GlobalAppException("Pending deletion not found!");

            cat.IsDeleted = true;
            cat.DeletedDate = DateTime.Now;
            cat.LastUpdatedDate = DateTime.Now;

            await _categoryWrite.UpdateAsync(cat);
            await _categoryWrite.CommitAsync();
        }

        public async Task RejectCategoryDeleteAsync(string categoryId)
        {
            var cat = await _categoryRead.GetAsync(
                c => c.Id.ToString() == categoryId
                  && c.Deleted
                  && !c.IsDeleted
            );
            if (cat == null)
                throw new GlobalAppException("Pending deletion not found!");

            cat.Deleted = false;
            cat.LastUpdatedDate = DateTime.Now;

            await _categoryWrite.UpdateAsync(cat);
            await _categoryWrite.CommitAsync();
        }

        // ─── APPROVE / REJECT UPDATE ─────────────────────────────────────────────────

        public async Task ApprovePendingCategoryAsync(string pendingId)
        {
            var pend = await _pendingRead.GetAsync(
                p => p.Id.ToString() == pendingId && !p.IsDeleted,
                include: q => q.Include(x => x.OriginalCategory)
            );
            if (pend == null)
                throw new GlobalAppException("Pending update not found!");

            var cat = pend.OriginalCategory!;
            cat.Name = pend.NewName!;
            cat.IsUpdated = true;
            cat.LastUpdatedDate = DateTime.Now;

            await _categoryWrite.UpdateAsync(cat);
            await _pendingWrite.HardDeleteAsync(pend);
            await _pendingWrite.CommitAsync();
        }

        public async Task RejectPendingCategoryAsync(string pendingId)
        {
            var pend = await _pendingRead.GetAsync(
                p => p.Id.ToString() == pendingId && !p.IsDeleted
            );
            if (pend == null)
                throw new GlobalAppException("Pending update not found!");

            await _pendingWrite.HardDeleteAsync(pend);
            await _pendingWrite.CommitAsync();
        }
    }
}
