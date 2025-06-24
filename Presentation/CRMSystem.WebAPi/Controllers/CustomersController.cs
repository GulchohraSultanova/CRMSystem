using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Application.Dtos.Customer;
using CRMSystem.Persistence.Concreters.Services;
using System.Security.Claims;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var result = await _customerService.CreateCustomerAsync(dto);
            return StatusCode(201, new { StatusCode = 201, Message = "Müştəri yaradıldı", Data = result });
        }
    
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAllCustomersAsync();
            return Ok(new { StatusCode = 200, Data = result });
        }
        [HttpDelete("{customerId}/sections/{sectionId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RemoveSection(string customerId, string sectionId)
        {
            await _customerService.RemoveSectionFromCustomerAsync(customerId, sectionId);
            return Ok(new { StatusCode = 200, Message = "Bölmə müştəridən silindi." });
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            return Ok(new { StatusCode = 200, Data = result });
        }
        [HttpPost("{customerId}/assign-sections")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AssignSections(string customerId, [FromBody] List<string> sectionIds)
        {
            await _customerService.AssignSectionsToCustomerAsync(customerId, sectionIds);
            return Ok(new { StatusCode = 200, Message = "Section-lar təyin edildi" });
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerDto dto)
        {
            var result = await _customerService.UpdateCustomerAsync(dto);
            return Ok(new { StatusCode = 200, Message = "Müştəri güncəlləndi", Data = result });
        }
        /// <summary>
        /// Login olmuş müştərinin şifrəsini dəyişir.
        /// </summary>
        [HttpPost("change-password")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { StatusCode = 400, Error = "Yanlış daxil edilmiş məlumat!" });

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _customerService.ChangePasswordAsync(customerId, dto);
                return Ok(new { StatusCode = 200, Message = "Şifrə uğurla dəyişdirildi." });
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
            await _customerService.DeleteCustomerAsync(id);
            return Ok(new { StatusCode = 200, Message = "Müştəri silindi" });
        }
        [HttpGet("companies")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyCompanies()
        {
            // token-dən userId oxuyuruq
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var companies = await _customerService.GetCompaniesByCustomerAsync(customerId);
            return Ok(new { StatusCode = 200, Data = companies });
        }

        /// <summary>
        /// Login olmuş müştərinin aid olduğu şöbələri gətirir.
        /// Query string-dən keçilən companyId ilə filtr etmək mümkündür.
        /// </summary>
        [HttpGet("departments")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyDepartments([FromQuery] string companyId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var departments = await _customerService.GetDepartmentsByCustomerAsync(customerId, companyId);
            return Ok(new { StatusCode = 200, Data = departments });
        }

        /// <summary>
        /// Login olmuş müştərinin aid olduğu bölmələri gətirir.
        /// Query string-dən keçilən departmentId ilə filtr etmək mümkündür.
        /// </summary>
        [HttpGet("sections")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMySections([FromQuery] string departmentId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sections = await _customerService.GetSectionsByCustomerAsync(customerId, departmentId);
            return Ok(new { StatusCode = 200, Data = sections });
        }
    }
}

