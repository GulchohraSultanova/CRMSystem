using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Calculation;
using CRMSystem.Application.GlobalAppException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Customer")]
    public class CalculationsController : ControllerBase
    {
        private readonly IMonthlyCalculationService _calculationService;
        private readonly ILogger<CalculationsController> _logger;

        public CalculationsController(IMonthlyCalculationService calculationService, ILogger<CalculationsController> logger)
        {
            _calculationService = calculationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOrUpdate([FromQuery] string companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BadRequest(new { StatusCode = 400, Error = "Şirkət identifikasiyası göndərilməyib!" });

                var result = await _calculationService.GenerateOrUpdateCurrentMonthCalculationAsync(companyId);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Cari ay üçün kalkulyasiya yaradıldı və ya yeniləndi.",
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Xəta baş verdi!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server xətası!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }

        [HttpGet("{year:int}/{month:int}")]
        public async Task<IActionResult> GetByMonth([FromQuery] string companyId, int year, int month)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BadRequest(new { StatusCode = 400, Error = "Şirkət identifikasiyası göndərilməyib!" });

                var result = await _calculationService.GetCalculationAsync(companyId, year, month);

                return Ok(new { StatusCode = 200, Data = result });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Kalkulyasiya tapılmadı!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Xəta baş verdi!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string companyId, int? year, int? month)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BadRequest(new { StatusCode = 400, Error = "Şirkət identifikasiyası göndərilməyib!" });

                var result = await _calculationService.FilterCalculationsAsync(companyId, year, month);

                return Ok(new { StatusCode = 200, Data = result });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Xəta baş verdi!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server xətası!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }

        [HttpPut("edit-initial")]
        public async Task<IActionResult> EditInitialAmount([FromQuery] string companyId, [FromQuery] int year, [FromQuery] int month, [FromQuery] decimal newAmount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BadRequest(new { StatusCode = 400, Error = "Şirkət identifikasiyası göndərilməyib!" });

                await _calculationService.EditInitialAmountAsync(companyId, year, month, newAmount);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "İlkin məbləğ uğurla yeniləndi."
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "İlkin məbləğ dəyişdirilə bilmədi!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server xətası!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }
    }
}
