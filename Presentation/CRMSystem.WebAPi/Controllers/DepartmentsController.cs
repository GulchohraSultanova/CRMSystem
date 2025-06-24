using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Department;
using CRMSystem.Application.GlobalAppException;
using Microsoft.AspNetCore.Http;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IDepartmentService departmentService, ILogger<DepartmentsController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            try
            {
                var result = await _departmentService.CreateDepartmentAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Şöbə uğurla yaradıldı!",
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şöbə yaradılarkən xəta baş verdi!");
                return BadRequest(new {   StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
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
            var result = await _departmentService.GetAllDepartmentAsync();
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
                var result = await _departmentService.GetDepartmentByIdAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şöbə tapılmadı.");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpGet("by-company/{companyId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetByCompanyId(string companyId)
        {
            try
            {
                var result = await _departmentService.GetDepartmentsByCompanyIdAsync(companyId);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şirkətə görə şöbələr alınarkən xəta baş verdi.");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update([FromBody] UpdateDepartmentDto dto)
        {
            try
            {
                var result = await _departmentService.UpdateDepartmentAsync(dto);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Şöbə uğurla güncəlləndi!",
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şöbə güncəllənərkən xəta baş verdi.");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest,Error = ex.Message });
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
                await _departmentService.DeleteDepartmentAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Şöbə uğurla silindi."
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Şöbə silinərkən xəta baş verdi.");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }
    }
}
