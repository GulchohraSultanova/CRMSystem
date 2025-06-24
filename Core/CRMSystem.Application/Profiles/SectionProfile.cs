// SectionProfile.cs
using AutoMapper;
using CRMSystem.Application.Dtos.Section;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Profiles
{
    public class SectionProfile : Profile
    {
        public SectionProfile()
        {
            // CreateSectionDto → Section
            CreateMap<CreateSectionDto, Section>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SectionImage, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => Guid.Parse(src.DepartmentId)));

            // UpdateSectionDto → Section
            CreateMap<UpdateSectionDto, Section>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SectionImage, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.DepartmentId)
                            ? (Guid?)null
                            : Guid.Parse(src.DepartmentId)));

            // Section → SectionDto
            CreateMap<Section, SectionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId.ToString()))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Department.Company.Name))
                .ForMember(dest => dest.Customers, opt => opt.MapFrom(src =>
                    src.CustomerList.Select(sc => new AdminInfoDto
                    {
                        Id = sc.AdminId,
                        Name = sc.Admin!.Name,
                        Surname = sc.Admin.Surname,
                        Email = sc.Admin.Email,
                        FinCode = sc.Admin.FinCode
                    }).ToList()
                ));
        }
    }
}
