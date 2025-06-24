using AutoMapper;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.Dtos.Job;
using CRMSystem.Application.Dtos.Job;
using CRMSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<CreateJobDto, Job>()
.ForMember(dest => dest.JobImage, opt => opt.Ignore())
.ForMember(dest => dest.Customers, opt => opt.Ignore()).ReverseMap();

            // UpdateJobDto → Job
            CreateMap<UpdateJobDto, Job>()
                .ForMember(dest => dest.JobImage, opt => opt.Ignore())
                .ForMember(dest => dest.Customers, opt => opt.Ignore()).ReverseMap();

            // Job → JobDto
            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
       
       
                .ForMember(dest => dest.Customers, opt => opt.MapFrom(src => src.Customers ?? new List<Admin>())).ReverseMap();

            // Job → JobDto (nested list)
            CreateMap<Admin, AdminInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
              
        }
    }
}
