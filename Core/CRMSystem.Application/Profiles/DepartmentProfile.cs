// DepartmentProfile.cs
using AutoMapper;
using CRMSystem.Application.Dtos.Department;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            // CreateDepartmentDto → Department
            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepartmentImage, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => Guid.Parse(src.CompanyId)));

            // UpdateDepartmentDto → Department
            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepartmentImage, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyId,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.CompanyId)
                            ? (Guid?)null
                            : Guid.Parse(src.CompanyId)));

            // Department → DepartmentDto
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId.ToString()))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.Sections, opt => opt.Ignore());
        }
    }
}
