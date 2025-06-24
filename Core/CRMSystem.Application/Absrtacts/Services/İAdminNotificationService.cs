using CRMSystem.Application.Dtos.Notification;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IAdminNotificationService
    {
        /// <summary>
        /// Create a new AdminNotification with the given type ("create"/"update"/"delete")
        /// and exactly one of categoryId or productId (the other should be null).
        /// </summary>
        Task CreateAsync(string type, string? categoryId, string? productId, string? orderId);

        Task<List<AdminNotificationDto>> GetAllAsync();
        Task<List<AdminNotificationDto>> GetUnreadAsync();
        Task<AdminNotificationDto> GetByIdAsync(string id);
        Task MarkAsReadAsync(string id);
    }
}
