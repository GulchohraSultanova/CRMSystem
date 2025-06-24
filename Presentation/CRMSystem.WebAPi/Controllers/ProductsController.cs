using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Product;
using CRMSystem.Application.GlobalAppException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ─── Public reads ─────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(new { StatusCode = 200, Data = list });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                return Ok(new { StatusCode = 200, Data = dto });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "GetById failed");
                return NotFound(new { StatusCode = 404, Error = ex.Message });
            }
        }
        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(string categoryId)
        {
            try
            {
                var list = await _service.GetProductsByCategoryAsync(categoryId);
                return Ok(new { StatusCode = 200, Data = list });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategoriya məhsulları alınarkən gözlənilməz xəta baş verdi.");
                return StatusCode(500, new { StatusCode = 500, Error = "Gözlənilməz xəta baş verdi." });
            }
        }
        // ─── Pending reads (SuperAdmin only) ─────────────────────────────────────

        [HttpGet("pending/adds")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingAdds()
        {
            var list = await _service.GetPendingAddsAsync();
            return Ok(new { StatusCode = 200, Data = list });
        }

        [HttpGet("pending/deletes")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingDeletes()
        {
            var list = await _service.GetPendingDeletesAsync();
            return Ok(new { StatusCode = 200, Data = list });
        }

        [HttpGet("pending/updates")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingUpdates()
        {
            var list = await _service.GetPendingUpdatesAsync();
            return Ok(new { StatusCode = 200, Data = list });
        }


        // ─── Requests (any authenticated user) ──────────────────────────────────

        [HttpPost("request-create")]
        [Authorize]
        public async Task<IActionResult> RequestCreate([FromBody] CreateProductDto dto)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
            await _service.RequestCreateAsync(role, dto);
            return StatusCode(201, new { StatusCode = 201, Message = "Create request submitted." });
        }

        [HttpPost("request-delete/{id}")]
        [Authorize]
        public async Task<IActionResult> RequestDelete(string id)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
            await _service.RequestDeleteAsync(role, id);
            return Ok(new { StatusCode = 200, Message = "Delete request submitted." });
        }

        [HttpPost("request-update")]
        [Authorize]
        public async Task<IActionResult> RequestUpdate([FromBody] UpdatePendingProductDto dto)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
            await _service.RequestUpdateAsync(role, dto);
            return Ok(new { StatusCode = 200, Message = "Update request submitted." });
        }

        // ─── Approve / Reject create (SuperAdmin only) ──────────────────────────

        [HttpPut("approve-create/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveCreate(string id)
        {
            await _service.ApproveCreateAsync(id);
            return Ok(new { StatusCode = 200, Message = "Product creation approved." });
        }

        [HttpDelete("reject-create/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectCreate(string id)
        {
            await _service.RejectCreateAsync(id);
            return Ok(new { StatusCode = 200, Message = "Product creation rejected." });
        }

        // ─── Approve / Reject delete ────────────────────────────────────────────

        [HttpPut("approve-delete/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveDelete(string id)
        {
            await _service.ApproveDeleteAsync(id);
            return Ok(new { StatusCode = 200, Message = "Product deletion approved." });
        }

        [HttpDelete("reject-delete/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectDelete(string id)
        {
            await _service.RejectDeleteAsync(id);
            return Ok(new { StatusCode = 200, Message = "Product deletion rejected." });
        }

        // ─── Approve / Reject update ────────────────────────────────────────────

        [HttpPut("approve-update/{pendingId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveUpdate(string pendingId)
        {
            await _service.ApproveUpdateAsync(pendingId);
            return Ok(new { StatusCode = 200, Message = "Product update approved." });
        }

        [HttpDelete("reject-update/{pendingId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectUpdate(string pendingId)
        {
            await _service.RejectUpdateAsync(pendingId);
            return Ok(new { StatusCode = 200, Message = "Product update rejected." });
        }
    }
}
