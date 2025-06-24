using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.OrderCustomer;
using CRMSystem.Application.GlobalAppException;
using System.Security.Claims;
using CRMSystem.Application.Dtos.OrderFighter;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(new { StatusCode = 401, Error = "İstifadəçi identifikasiyası tapılmadı!" });

                var result = await _orderService.CreateOrderAsync(adminId, dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Sifariş uğurla yaradıldı!",
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Sifariş yaradılarkən xəta baş verdi!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gözlənilməz xəta!");
                return StatusCode(500, new { StatusCode = 500, Error = $"Xəta baş verdi: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Fighter")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }

        [HttpPost("fighter")]
        [Authorize(Roles = "Fighter")]
        public async Task<IActionResult> CreateFighterOrder([FromForm] OrderFighterDto dto)
        {
            try
            {
                var fighterId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(fighterId))
                    return Unauthorized(new { StatusCode = 401, Error = "Təchizatçı identifikasiyası tapılmadı!" });

                if (dto.OrderItems == null || !dto.OrderItems.Any())
                    return BadRequest(new { StatusCode = 400, Error = "OrderItems siyahısı boşdur!" });

                var result = await _orderService.CreateOrderFighterAsync(fighterId, dto);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Təchizatçı sifarişi uğurla yaratdı!",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Xəta baş verdi!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyOrders()
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(new { StatusCode = 401, Error = "İstifadəçi identifikasiyası tapılmadı!" });

                var orders = await _orderService.GetOrdersByAdminIdAsync(adminId);

                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = orders
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Adminin sifarişləri tapılmadı!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gözlənilməz xəta!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }

        [HttpPost("confirm-delivery/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ConfirmOrderDelivery(string orderId)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(new { StatusCode = 401, Error = "İstifadəçi identifikasiyası tapılmadı!" });

                await _orderService.ConfirmOrderDeliveryAsync(orderId);

                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Sifariş təhvili təsdiqləndi."
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Sifariş tapılmadı və ya təsdiq edilə bilmədi!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gözlənilməz xəta!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }

        [HttpDelete("{orderId}")]
        [Authorize(Roles = "Customer,Fighter")]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(new { StatusCode = 401, Error = "İstifadəçi identifikasiyası tapılmadı!" });

                await _orderService.DeleteOrderByAdminAsync(orderId, adminId);

                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Sifariş uğurla silindi."
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Sifariş silinərkən xəta baş verdi!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gözlənilməz xəta!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }


        [HttpDelete("fighter/{orderId}")]
        [Authorize(Roles = "Customer,Fighter")]
        public async Task<IActionResult> DeleteOrderByFighter(string orderId)
        {
            try
            {
           

                await _orderService.DeleteOrderByFighterAsync(orderId);

                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Sifariş uğurla silindi."
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Sifariş silinərkən xəta baş verdi!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gözlənilməz xəta!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }
        [HttpGet("by-vendor")]
        [Authorize(Roles = "SuperAdmin,Fighter")]
        public async Task<IActionResult> GetOrdersByVendorId([FromQuery] string vendorId)
        {
            try
            {
                if (string.IsNullOrEmpty(vendorId))
                    return BadRequest(new { StatusCode = 400, Error = "VendorId boş ola bilməz." });

                var orders = await _orderService.GetOrdersByVendorIdAsync(vendorId);

                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = orders
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Vendor üzrə sifarişlər tapılmadı!");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gözlənilməz xəta!");
                return StatusCode(500, new { StatusCode = 500, Error = ex.Message });
            }
        }



        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,Customer,Fighter")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _orderService.GetOrderByIdAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Sifariş tapılmadı!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Error = $"Xəta baş verdi: {ex.Message}" });
            }
        }
    }
}
