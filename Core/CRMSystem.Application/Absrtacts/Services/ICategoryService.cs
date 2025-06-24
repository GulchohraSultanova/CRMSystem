using CRMSystem.Application.Dtos.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface ICategoryService
    {
        // Publicly visible categories
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(string id);

        // Pending lists (SuperAdmin only)
        Task<List<CategoryDto>> GetAllPendingAddsAsync();
        Task<List<CategoryDto>> GetAllPendingDeletesAsync();
        Task<List<PendingCategoryDetailsDto>> GetAllPendingUpdatesAsync();


        // “Request” (Fighter or SuperAdmin)
        Task RequestCreateCategoryAsync(string role, CreateCategoryDto dto);
        Task RequestDeleteCategoryAsync(string role, string id);
        Task RequestUpdateCategoryAsync(string role, UpdatePendingCategoryDto dto);

        // SuperAdmin workflows: approve/reject
        Task ApproveCategoryCreateAsync(string categoryId);
        Task RejectCategoryCreateAsync(string categoryId);

        Task ApproveCategoryDeleteAsync(string categoryId);
        Task RejectCategoryDeleteAsync(string categoryId);

        Task ApprovePendingCategoryAsync(string pendingId);
        Task RejectPendingCategoryAsync(string pendingId);
    }
}
