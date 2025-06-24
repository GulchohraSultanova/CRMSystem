using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.Jobs;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Job;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class JobService : IJobService
    {
        private readonly IJobReadRepository _readRepository;
        private readonly IJobWriteRepository _writeRepository;
        private readonly IMediaService _fileService;
        private readonly IMapper _mapper;

        public JobService(
            IJobReadRepository readRepository,
            IJobWriteRepository writeRepository,
            IMediaService fileService,
            IMapper mapper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<JobDto> CreateJobAsync(CreateJobDto dto)
        {
            var job = _mapper.Map<Job>(dto);
            job.CreatedDate = DateTime.Now;

        

            await _writeRepository.AddAsync(job);
            await _writeRepository.CommitAsync();

            return _mapper.Map<JobDto>(job);
        }

        public async Task<List<JobDto>> GetAllJobsAsync()
        {
            var jobs = await _readRepository.GetAllAsync(
                j => !j.IsDeleted,
                include: q => q.Include(j => j.Customers)
            );

            return _mapper.Map<List<JobDto>>(jobs);
        }

        public async Task<JobDto> GetJobByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid jobId))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var job = await _readRepository.GetAsync(
                j => j.Id == jobId && !j.IsDeleted,
                include: q => q.Include(j => j.Customers)
            );

            if (job == null)
                throw new GlobalAppException("İş tapılmadı!");

            return _mapper.Map<JobDto>(job);
        }



        public async Task<JobDto> UpdateJobAsync(UpdateJobDto dto)
        {
            if (!Guid.TryParse(dto.Id, out Guid jobId))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var job = await _readRepository.GetAsync(j => j.Id == jobId && !j.IsDeleted);
            if (job == null)
                throw new GlobalAppException("İş tapılmadı!");

            job.Name = dto.Name ?? job.Name;

     

            job.LastUpdatedDate = DateTime.Now;

      

            await _writeRepository.UpdateAsync(job);
            await _writeRepository.CommitAsync();

            return _mapper.Map<JobDto>(job);
        }

        public async Task DeleteJobAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid jobId))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var job = await _readRepository.GetAsync(j => j.Id == jobId && !j.IsDeleted);
            if (job == null)
                throw new GlobalAppException("İş tapılmadı!");

            job.IsDeleted = true;
            job.DeletedDate = DateTime.Now;

            await _writeRepository.UpdateAsync(job);
            await _writeRepository.CommitAsync();
        }
    }
}
