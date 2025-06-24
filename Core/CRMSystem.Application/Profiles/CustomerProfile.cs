// CustomerProfile.cs
using AutoMapper;
using CRMSystem.Application.Dtos.Customer;
using CRMSystem.Domain.Entities;
using System;

namespace CRMSystem.Application.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // CreateCustomerDto → Admin
            CreateMap<CreateCustomerDto, Admin>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.FinCode, opt => opt.MapFrom(src => src.FinCode))
                .ForMember(dest => dest.JobId,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.JobId)
                            ? (Guid?)null
                            : Guid.Parse(src.JobId)));

            // UpdateCustomerDto → Admin
            CreateMap<UpdateCustomerDto, Admin>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
            
                .ForMember(dest => dest.FinCode, opt => opt.MapFrom(src => src.FinCode))
                .ForMember(dest => dest.JobId,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.JobId)
                            ? (Guid?)null
                            : Guid.Parse(src.JobId)));

            // Admin → CustomerDto
            CreateMap<Admin, CustomerDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.FinCode, opt => opt.MapFrom(src => src.FinCode))
                .ForMember(dest => dest.JobId,
                    opt => opt.MapFrom(src => src.JobId.HasValue ? src.JobId.Value.ToString() : null))
                .ForMember(dest => dest.Sections, opt => opt.Ignore())  // build metodda ayrıca yüklənir
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
