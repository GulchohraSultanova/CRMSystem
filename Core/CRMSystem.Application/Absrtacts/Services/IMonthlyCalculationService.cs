// Interface
using CRMSystem.Application.Dtos.Calculation;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IMonthlyCalculationService
    {
        Task<CalculationDto> GenerateOrUpdateCurrentMonthCalculationAsync(string companyId);
        Task<CalculationDto> GetCalculationAsync(string companyId, int year, int month);
        Task<List<CalculationDto>> FilterCalculationsAsync(string companyId, int? year = null, int? month = null);
        Task EditInitialAmountAsync(string companyId, int year, int month, decimal newAmount);
    }
}

