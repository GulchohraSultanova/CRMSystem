using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Company;
using CRMSystem.Application.GlobalAppException;

namespace CRMSystem.WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ICompanyService companyService, ILogger<CompaniesController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
        {
            try
            {
                var result = await _companyService.CreateCompanyAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Şirkət uğurla yaradıldı!",
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şirkət yaradılarkən xəta baş verdi!");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _companyService.GetAllCompanyAsync();
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _companyService.GetCompanyByIdAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şirkət tapılmadı!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyDto dto)
        {
            try
            {
                var updated = await _companyService.UpdateCompanyAsync(dto);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Şirkət uğurla güncəlləndi!",
                    Data = updated
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şirkət güncəllənərkən xəta baş verdi!");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _companyService.DeleteCompanyAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Şirkət uğurla silindi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şirkət silinərkən xəta baş verdi!");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }
    }
}
