using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.GlobalAppException;
using System.Security.Claims;

namespace CRMSystem.WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightersController : ControllerBase
    {
        private readonly IFighterService _fighterService;
        private readonly ILogger<FightersController> _logger;

        public FightersController(IFighterService fighterService, ILogger<FightersController> logger)
        {
            _fighterService = fighterService;
            _logger = logger;
        }

        [HttpPost("register")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                await _fighterService.RegisterFighterAsync(registerDto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Fighter uğurla yaradıldı!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Fighter qeydiyyatı zamanı xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpPost("change-password")]
        [Authorize(Roles = "Fighter")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { StatusCode = 400, Error = "Yanlış daxil edilmiş məlumat!" });

            var fighterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _fighterService.ChangePasswordAsync(fighterId, dto);
                return Ok(new { StatusCode = 200, Message = "Şifrə uğurla dəyişdirildi." });
            }
            catch (GlobalAppException ex)
            {
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update([FromBody] UpdateFighterDto dto)
        {
            try
            {
                var updatedDto = await _fighterService.UpdateFighterAsync(dto);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Fighter uğurla güncəlləndi!",
                    Data = updatedDto
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Update zamanı xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
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
            try
            {
                var fighters = await _fighterService.GetAllFightersAsync();
                return Ok(new { StatusCode = 200, Data = fighters });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var fighter = await _fighterService.GetFighterByIdAsync(id);
                return Ok(new { StatusCode = 200, Data = fighter });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Fighter tapılmadı.");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }


        [HttpDelete("{email}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteByEmail(string email)
        {
            try
            {
                await _fighterService.DeleteFighterByEmailAsync(email);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Fighter ({email}) uğurla silindi."
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Fighter silinərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Gözlənilməz xəta baş verdi: {ex.Message}" });
            }
        }
    }
}
