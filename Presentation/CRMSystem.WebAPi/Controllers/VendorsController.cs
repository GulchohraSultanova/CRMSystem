using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Vendor;
using CRMSystem.Application.GlobalAppException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly ILogger<VendorsController> _logger;

        public VendorsController(IVendorService vendorService, ILogger<VendorsController> logger)
        {
            _vendorService = vendorService;
            _logger = logger;
        }

        // ─── CREATE ────────────────────────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVendorDto dto)
        {
            try
            {
                var created = await _vendorService.CreateVendorAsync(dto);
                return Ok(  new { StatusCode = 201, Data = created });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Vendor yaradılarkən xəta");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }

        // ─── READ ALL ───────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _vendorService.GetAllVendorsAsync();
            return Ok(new { StatusCode = 200, Data = list });
        }

        // ─── READ BY ID ─────────────────────────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var dto = await _vendorService.GetVendorByIdAsync(id);
                return Ok(new { StatusCode = 200, Data = dto });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Vendor tapılmadı");
                return NotFound(new { StatusCode = 404, Error = ex.Message });
            }
        }

        // ─── UPDATE ────────────────────────────────────────────────────────────────
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateVendorDto dto)
        {
            try
            {
                var updated = await _vendorService.UpdateVendorAsync(dto);
                return Ok(new { StatusCode = 200, Data = updated });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Vendor güncəllənərkən xəta");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }

        // ─── DELETE ────────────────────────────────────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _vendorService.DeleteVendorAsync(id);
                return Ok(new { StatusCode = 200, Message = "Vendor silindi." });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Vendor silinərkən xəta");
                return NotFound(new { StatusCode = 404, Error = ex.Message });
            }
        }
    }
}
