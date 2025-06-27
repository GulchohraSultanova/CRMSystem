using CRMSystem.Application.Absrtacts.Repositories.Calculator;
using CRMSystem.Application.Absrtacts.Repositories.Orders;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

public class MonthlyCalculationService : IMonthlyCalculationService
{
    private readonly IMonthlyCalculationReadRepository _calcRead;
    private readonly IMonthlyCalculationWriteRepository _calcWrite;
    private readonly IOrderReadRepository _orderRead;

    public MonthlyCalculationService(
        IMonthlyCalculationReadRepository calcRead,
        IMonthlyCalculationWriteRepository calcWrite,
        IOrderReadRepository orderRead)
    {
        _calcRead = calcRead;
        _calcWrite = calcWrite;
        _orderRead = orderRead;
    }

    public async Task<CalculationDto> GenerateOrUpdateCurrentMonthCalculationAsync(string companyId)
    {
        if (!Guid.TryParse(companyId, out var companyGuid))
            throw new GlobalAppException("Company ID düzgün formatda deyil!");

        var now = DateTime.UtcNow;
        var year = now.Year;
        var month = now.Month;

        var existing = await _calcRead.GetAsync(x =>
            x.CompanyId == companyGuid && x.Year == year && x.Month == month);

        decimal previousFinal = 0;

        var prev = await _calcRead.GetAsync(x =>
            x.CompanyId == companyGuid &&
            (x.Year < year || (x.Year == year && x.Month < month)),
            orderBy: q => q.OrderByDescending(c => c.Year).ThenByDescending(c => c.Month));

        if (prev != null)
            previousFinal = prev.TotalAmount;

        var orderSum = await _orderRead.GetAllAsync(
            o => o.Section.Department.CompanyId == companyGuid &&
                 o.CreatedDate.Year == year && o.CreatedDate.Month == month &&
                 o.FighterConfirm && !o.IsDeleted,
            include: q => q.Include(x => x.Items).Include(s=>s.Section).ThenInclude(s=>s.Department).ThenInclude(s=>s.Company));

        var totalOrders = orderSum.SelectMany(o => o.Items).Sum(i => i.Price ?? 0);

        if (existing != null)
        {
            existing.InitialAmount = previousFinal;
            existing.TotalOrderAmount = totalOrders;
            existing.LastUpdatedDate = DateTime.UtcNow;
            await _calcWrite.UpdateAsync(existing);
        }
        else
        {
            existing = new MonthlyCalculation
            {
                Id = Guid.NewGuid(),
                CompanyId = companyGuid,
                Year = year,
                Month = month,
                InitialAmount = previousFinal,
                TotalOrderAmount = totalOrders,
                CreatedDate = DateTime.UtcNow
            };
            await _calcWrite.AddAsync(existing);
        }

        await _calcWrite.CommitAsync();

        return new CalculationDto
        {
            MonthName = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(month),
            InitialAmount = existing.InitialAmount,
            OrderTotalAmount = existing.TotalOrderAmount,
            TotalAmount = existing.TotalAmount
        };
    }

    public async Task<CalculationDto> GetCalculationAsync(string companyId, int year, int month)
    {
        if (!Guid.TryParse(companyId, out var companyGuid))
            throw new GlobalAppException("Company ID düzgün formatda deyil!");

        var calc = await _calcRead.GetAsync(x =>
            x.CompanyId == companyGuid && x.Year == year && x.Month == month);

        if (calc == null)
            throw new GlobalAppException("Bu ay üçün kalkulyasiya tapılmadı!");

        return new CalculationDto
        {
            MonthName = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(calc.Month),
            InitialAmount = calc.InitialAmount,
            OrderTotalAmount = calc.TotalOrderAmount,
 
            TotalAmount = calc.TotalAmount
        };
    }

    public async Task<List<CalculationDto>> FilterCalculationsAsync(string companyId, int? year = null, int? month = null)
    {
        if (!Guid.TryParse(companyId, out var companyGuid))
            throw new GlobalAppException("Company ID düzgün formatda deyil!");

        var all = await _calcRead.GetAllAsync(x =>
            x.CompanyId == companyGuid &&
            (!year.HasValue || x.Year == year) &&
            (!month.HasValue || x.Month == month));

        return all.Select(x => new CalculationDto
        {
            MonthName = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(x.Month),
            InitialAmount = x.InitialAmount,
            OrderTotalAmount = x.TotalOrderAmount,
            TotalAmount = x.TotalAmount
        }).ToList();
    }

    public async Task EditInitialAmountAsync(string companyId, int year, int month, decimal newAmount)
    {
        if (!Guid.TryParse(companyId, out var companyGuid))
            throw new GlobalAppException("Company ID düzgün formatda deyil!");

        var calc = await _calcRead.GetAsync(x =>
            x.CompanyId == companyGuid && x.Year == year && x.Month == month);

        if (calc == null)
            throw new GlobalAppException("Göstərilən ay üçün kalkulyasiya tapılmadı!");

        calc.InitialAmount = newAmount;
        calc.LastUpdatedDate = DateTime.UtcNow;

        await _calcWrite.UpdateAsync(calc);
        await _calcWrite.CommitAsync();
    }
}
