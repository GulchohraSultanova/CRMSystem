using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.GlobalAppException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAdminService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IAdminService accountService,
                              ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Error = "Yanlış daxil etmə məlumatı!"
            });
        }

        try
        {
            var tokenResponse = await _accountService.LoginFighterOrCustomerAsync(loginDto);
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Data = tokenResponse
            });
        }
        catch (GlobalAppException ex)
        {
            _logger.LogError(ex, "Login zamanı xəta baş verdi!");
            return BadRequest(new
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Error = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login zamanı gözlənilməz xəta baş verdi!");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin."
            });
        }
    }
}
