using CRMSystem.Application.Absrtacts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<StatisticsController> _logger;

    public StatisticsController(IOrderService orderService, ILogger<StatisticsController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet("order-status-percentages/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetOrderStatusPercentages(string companyId)
    {
        try
        {
            var (deletedPercent, completedPercent, waitingPercent) = await _orderService.GetOrderStatusPercentagesAsync(companyId);

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                DeletedPercent = Math.Round(deletedPercent, 2),
                CompletedPercent = Math.Round(completedPercent, 2),
                WaitingPercent = Math.Round(waitingPercent, 2)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Statistika alınarkən xəta baş verdi!");
            return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
        }
    }

    [HttpGet("monthly-order-status/{year}/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetMonthlyOrderStatusCounts(int year, string companyId)
    {
        try
        {
            var result = await _orderService.GetMonthlyOrderStatusCountsAsync(year, companyId);
            return Ok(new { StatusCode = 200, StatusCounts = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Statuslara əsasən statistikada xəta baş verdi.");
            return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
        }
    }

    [HttpGet("monthly-order-amounts/{year}/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetMonthlyOrderAmounts(int year, string companyId)
    {
        try
        {
            var result = await _orderService.GetMonthlyOrderAmountsByYearAsync(year, companyId);
            return Ok(new { StatusCode = 200, MonthlyOrderAmounts = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sifariş qiymətləri statistikası alınarkən xəta baş verdi!");
            return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
        }
    }

    [HttpGet("monthly-orders/{year}/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetMonthlyOrders(int year, string companyId)
    {
        try
        {
            var result = await _orderService.GetMonthlyOrderCountsByYearAsync(year, companyId);
            return Ok(new { StatusCode = 200, MonthlyOrders = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Statistika alınarkən xəta baş verdi!");
            return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
        }
    }

    [HttpGet("fighter-order-status/{fighterId}/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetFighterOrderStatus(string fighterId, string companyId)
    {
        try
        {
            var (completed, canceled, waiting) = await _orderService.GetFighterOrderStatusPercentagesAsync(fighterId, companyId);
            return Ok(new
            {
                StatusCode = 200,
                CompletedPercent = Math.Round(completed, 2),
                CanceledPercent = Math.Round(canceled, 2),
                WaitingPercent = Math.Round(waiting, 2)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fighter statistikası alınarkən xəta baş verdi!");
            return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
        }
    }

    [HttpGet("fighter-order-count/{fighterId}/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetFighterConfirmedOrderCount(string fighterId, string companyId)
    {
        try
        {
            var count = await _orderService.GetFighterConfirmedOrderCountAsync(fighterId, companyId);
            return Ok(new { StatusCode = 200, ConfirmedOrderCount = count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fighter təsdiqlənmiş sifariş sayı alınarkən xəta baş verdi!");
            return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
        }
    }

    [HttpGet("fighter-monthly-completion/{fighterId}/{year}/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetFighterMonthlyCompletion(string fighterId, int year, string companyId)
    {
        try
        {
            var result = await _orderService.GetFighterMonthlyCompletedAndIncompleteOrdersAsync(fighterId, year, companyId);
            return Ok(new { StatusCode = 200, MonthlyCompletionStats = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fighter aylıq statistikası alınarkən xəta baş verdi!");
            return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
        }
    }

    [HttpGet("total-orders/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetFighterConfirmedOrdersStatistics(string companyId)
    {
        try
        {
            var stats = await _orderService.GetFighterConfirmedOrdersStatisticsAsync(companyId);
            return Ok(new
            {
                StatusCode = 200,
                TotalConfirmedOrders = stats.TotalConfirmedOrders,
                PercentGrowth = Math.Round(stats.PercentGrowth, 2)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Statistika alınarkən xəta baş verdi!");
            return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
        }
    }
}
