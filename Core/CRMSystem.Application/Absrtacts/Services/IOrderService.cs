using CRMSystem.Application.Dtos.OrderCustomer;
using CRMSystem.Application.Dtos.OrderFighter;
using CRMSystem.Application.Dtos.Statistic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string adminId, CreateOrderDto dto);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(string id);
        Task<OrderDto> CreateOrderFighterAsync(string adminId, OrderFighterDto dto);
        Task<List<OrderDto>> GetOrdersByAdminIdAsync(string adminId);
        Task DeleteOrderByAdminAsync(string orderId, string adminId);
        Task<List<OrderDto>> GetOrdersByVendorIdAsync(string vendorId);
        Task DeleteOrderByFighterAsync(string orderId);
        Task ConfirmOrderDeliveryAsync(string orderId);

        // ✅ Şirkətə görə statistik metodlar (companyId əlavə olundu):
        Task<List<MonthlyOrderStatusDto>> GetMonthlyOrderStatusCountsAsync(int year, string companyId);
        Task<Dictionary<string, int>> GetMonthlyOrderCountsByYearAsync(int year, string companyId);
        Task<Dictionary<string, decimal>> GetMonthlyOrderAmountsByYearAsync(int year, string companyId);
        Task<(int TotalConfirmedOrders, decimal PercentGrowth)> GetFighterConfirmedOrdersStatisticsAsync(string companyId);
        Task<(decimal DeletedPercent, decimal CompletedPercent, decimal WaitingPercent)> GetOrderStatusPercentagesAsync(string companyId);

        // ✅ Fighter və şirkətə görə statistikalar:
        Task<int> GetFighterConfirmedOrderCountAsync(string fighterId, string companyId);
        Task<List<MonthlyFighterOrderStatusDto>> GetFighterMonthlyCompletedAndIncompleteOrdersAsync(string fighterId, int year, string companyId);
        Task<(decimal CompletedPercent, decimal CanceledPercent, decimal WaitingPercent)> GetFighterOrderStatusPercentagesAsync(string fighterId, string companyId);

        // ✅ Məhsula görə statistikalar:
        Task<decimal> GetTotalSuppliedQuantityByProductIdAsync(string productId, string companyId);
        Task<Dictionary<string, decimal>> GetMonthlySuppliedQuantitiesByProductAsync(string productId, int year, string companyId);
    }
}
