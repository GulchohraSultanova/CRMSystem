using AutoMapper;
using CRMSystem.Domain.Entities;
using CRMSystem.Application.Dtos.Company;

namespace CRMSystem.Application.MappingProfiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            // Entity <-> View DTO
            CreateMap<Company, CompanyDto>()

             
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())).ReverseMap();

            // Entity <-> Create DTO
            CreateMap<Company, CreateCompanyDto>()
            
                .ReverseMap();

            // Entity <-> Update DTO
            CreateMap<Company, UpdateCompanyDto>()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
              
                .ReverseMap()
                .ForMember(dest => dest.Departments, opt => opt.Ignore());
        }
    }
}
