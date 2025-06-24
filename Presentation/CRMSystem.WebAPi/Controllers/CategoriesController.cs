using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Category;
using CRMSystem.Application.GlobalAppException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService service, ILogger<CategoriesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ─── Public read ────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllCategoriesAsync();
            try
            {
                return Ok(new { StatusCode = 200, Data = list });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
         

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var dto = await _service.GetCategoryByIdAsync(id);
                return Ok(new { StatusCode = 200, Data = dto });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { StatusCode = 404, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        // ─── Pending reads (SuperAdmin) ────────────────────────────────────────────

        [HttpGet("pending/adds")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingAdds()
        {
            var list = await _service.GetAllPendingAddsAsync();
            try
            {
                return Ok(new { StatusCode = 200, Data = list });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpGet("pending/deletes")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingDeletes()
        {
            var list = await _service.GetAllPendingDeletesAsync();
            try
            {
                return Ok(new { StatusCode = 200, Data = list });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpGet("pending/updates")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingUpdates()
        {
            var list = await _service.GetAllPendingUpdatesAsync();
            try
            {
                return Ok(new { StatusCode = 200, Data = list });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }


        // ─── Request endpoints (any authenticated) ─────────────────────────────────

        [HttpPost("request-create")]
        [Authorize]
        public async Task<IActionResult> RequestCreate([FromBody] CreateCategoryDto dto)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
            await _service.RequestCreateCategoryAsync(role, dto);
            try
            {
                return StatusCode(201, new { StatusCode = 201, Message = "Create request submitted." });
            }
              catch (Exception ex)
  {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpPost("request-delete/{id}")]
        [Authorize]
        public async Task<IActionResult> RequestDelete(string id)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
            await _service.RequestDeleteCategoryAsync(role, id);
            try
            {
                return Ok(new { StatusCode = 200, Message = "Delete request submitted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }

        }

        [HttpPost("request-update")]
        [Authorize]
        public async Task<IActionResult> RequestUpdate([FromBody] UpdatePendingCategoryDto dto)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
            await _service.RequestUpdateCategoryAsync(role, dto);
            try
            {
                return Ok(new { StatusCode = 200, Message = "Update request submitted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        // ─── Approve / Reject create (SuperAdmin) ──────────────────────────────────

        [HttpPut("approve-create/{categoryId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveCreate(string categoryId)
        {
            await _service.ApproveCategoryCreateAsync(categoryId);
            try
            {
                return Ok(new { StatusCode = 200, Message = "Category creation approved." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpDelete("reject-create/{categoryId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectCreate(string categoryId)
        {
            await _service.RejectCategoryCreateAsync(categoryId);
            try
            {
                return Ok(new { StatusCode = 200, Message = "Category creation rejected." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        // ─── Approve / Reject delete ────────────────────────────────────────────────

        [HttpPut("approve-delete/{categoryId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveDelete(string categoryId)
        {
            await _service.ApproveCategoryDeleteAsync(categoryId);
            return Ok(new { StatusCode = 200, Message = "Category deletion approved." });
        }

        [HttpDelete("reject-delete/{categoryId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectDelete(string categoryId)
        {
            await _service.RejectCategoryDeleteAsync(categoryId);
            return Ok(new { StatusCode = 200, Message = "Category deletion rejected." });
        }

        // ─── Approve / Reject update ────────────────────────────────────────────────

        [HttpPut("approve-update/{pendingId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveUpdate(string pendingId)
        {
            await _service.ApprovePendingCategoryAsync(pendingId);
            return Ok(new { StatusCode = 200, Message = "Category update approved." });
        }

        [HttpDelete("reject-update/{pendingId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectUpdate(string pendingId)
        {
            await _service.RejectPendingCategoryAsync(pendingId);
            return Ok(new { StatusCode = 200, Message = "Category update rejected." });
        }
    }
}
