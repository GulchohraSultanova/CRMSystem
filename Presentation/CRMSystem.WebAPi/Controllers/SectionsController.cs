using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Section;
using CRMSystem.Application.GlobalAppException;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionService _sectionService;
        private readonly ILogger<SectionsController> _logger;

        public SectionsController(ISectionService sectionService, ILogger<SectionsController> logger)
        {
            _sectionService = sectionService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateSectionDto dto)
        {
            try
            {
                var result = await _sectionService.CreateSectionAsync(dto);
                return StatusCode(201, new { StatusCode = 201, Message = "Bölmə yaradıldı!", Data = result });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Bölmə yaradılarkən xəta!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sectionService.GetAllSectionsAsync();
            return Ok(new { StatusCode = 200, Data = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _sectionService.GetSectionByIdAsync(id);
                return Ok(new { StatusCode = 200, Data = result });
            }
            catch (GlobalAppException ex)
            {
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }

        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetByDepartmentId(string departmentId)
        {
            try
            {
                var result = await _sectionService.GetSectionsByDepartmentIdAsync(departmentId);
                return Ok(new { StatusCode = 200, Data = result });
            }
            catch (GlobalAppException ex)
            {
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update([FromBody] UpdateSectionDto dto)
        {
            try
            {
                var result = await _sectionService.UpdateSectionAsync(dto);
                return Ok(new { StatusCode = 200, Message = "Bölmə güncəlləndi!", Data = result });
            }
            catch (GlobalAppException ex)
            {
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _sectionService.DeleteSectionAsync(id);
                return Ok(new { StatusCode = 200, Message = "Bölmə silindi!" });
            }
            catch (GlobalAppException ex)
            {
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }
    }
}
