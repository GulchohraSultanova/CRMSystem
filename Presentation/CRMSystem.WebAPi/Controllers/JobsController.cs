using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Job;
using CRMSystem.Application.GlobalAppException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IJobService jobService, ILogger<JobsController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateJobDto dto)
        {
            try
            {
                var result = await _jobService.CreateJobAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new { StatusCode = 201, Data = result });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "İş yaradılarkən xəta baş verdi.");
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _jobService.GetAllJobsAsync();
            return Ok(new { StatusCode = 200, Data = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _jobService.GetJobByIdAsync(id);
                return Ok(new { StatusCode = 200, Data = result });
            }
            catch (GlobalAppException ex)
            {
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }



        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update([FromBody] UpdateJobDto dto)
        {
            try
            {
                var result = await _jobService.UpdateJobAsync(dto);
                return Ok(new { StatusCode = 200, Message = "İş uğurla güncəlləndi!", Data = result });
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
                await _jobService.DeleteJobAsync(id);
                return Ok(new { StatusCode = 200, Message = "İş uğurla silindi." });
            }
            catch (GlobalAppException ex)
            {
                return BadRequest(new { StatusCode = 400, Error = ex.Message });
            }
        }
    }
}
