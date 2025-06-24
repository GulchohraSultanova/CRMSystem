using CRMSystem.Application.Dtos.Job;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IJobService
    {
        Task<JobDto> CreateJobAsync(CreateJobDto dto);
        Task<List<JobDto>> GetAllJobsAsync();
        Task<JobDto> GetJobByIdAsync(string id);
 
        Task<JobDto> UpdateJobAsync(UpdateJobDto dto);
        Task DeleteJobAsync(string id);
    }
}
