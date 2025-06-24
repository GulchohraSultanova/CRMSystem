using CRMSystem.Application.Dtos.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IProductService
    {
        // approved, created & not deleted
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(string id);

        Task<List<ProductDto>> GetProductsByCategoryAsync(string categoryId);

        // pending lists (SuperAdmin)
        Task<List<ProductDto>> GetPendingAddsAsync();
        Task<List<ProductDto>> GetPendingDeletesAsync();
        Task<List<PendingProductDetailsDto>> GetPendingUpdatesAsync();


        // any authenticated user can request → role is passed in
        Task RequestCreateAsync(string role, CreateProductDto dto);
        Task RequestDeleteAsync(string role, string id);
        Task RequestUpdateAsync(string role, UpdatePendingProductDto dto);

        // SuperAdmin only
        Task ApproveCreateAsync(string id);
        Task RejectCreateAsync(string id);

        Task ApproveDeleteAsync(string id);
        Task RejectDeleteAsync(string id);

        Task ApproveUpdateAsync(string pendingId);
        Task RejectUpdateAsync(string pendingId);
    }
}
