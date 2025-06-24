using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Notification;
using CRMSystem.Application.GlobalAppException;
using System.Threading.Tasks;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminNotificationsController : ControllerBase
    {
        private readonly IAdminNotificationService _notificationService;
        private readonly ILogger<AdminNotificationsController> _logger;

        public AdminNotificationsController(
            IAdminNotificationService notificationService,
            ILogger<AdminNotificationsController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Bütün bildirişləri gətirir (oxunmuş və oxunmamış).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _notificationService.GetAllAsync();
            return Ok(new { StatusCode = 200, Data = list });
        }

        /// <summary>
        /// Yalnız oxunmamış bildirişləri gətirir.
        /// </summary>
        [HttpGet("unread")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetUnread()
        {
            var list = await _notificationService.GetUnreadAsync();
            return Ok(new { StatusCode = 200, Data = list });
        }

        /// <summary>
        /// Verilmiş ID-li bildirişi gətirir.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var dto = await _notificationService.GetByIdAsync(id);
                return Ok(new { StatusCode = 200, Data = dto });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, $"Notification (ID={id}) tapılmadı.");
                return NotFound(new { StatusCode = 404, Error = ex.Message });
            }
        }

        /// <summary>
        /// Verilmiş ID-li bildirişi “oxunmuş” kimi işarələyir.
        /// </summary>
        [HttpPut("mark-as-read/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(id);
                return Ok(new { StatusCode = 200, Message = "Bildiriş oxunmuş kimi işarələndi." });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, $"Notification (ID={id}) tapılmadı.");
                return NotFound(new { StatusCode = 404, Error = ex.Message });
            }
        }
    }
}
