using AutoMapper;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            // RegisterDto -> Admin
            CreateMap<RegisterDto, Admin>()
                 .ForMember(dest => dest.UserName,
                       opt => opt.MapFrom(src => $"{src.PhoneNumber}@gmail.com"))
            .ForMember(dest => dest.Email,
                       opt => opt.MapFrom(src => $"{src.PhoneNumber}@gmail.com"))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname)).ReverseMap();
            // ID Identity tarafından atanır
            // Admin -> AdminInfoDto
            CreateMap<Admin, AdminInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Password, opt => opt.Ignore()); // Şifrə təhlükəsizlik üçün boş göndərilir

            // LoginDto -> Admin (Sadece mapleme için, Identity doğrulaması için kullanılmaz)
            CreateMap<LoginDto, Admin>()
                   .ForMember(dest => dest.UserName,
                       opt => opt.MapFrom(src => $"{src.PhoneNumber}@gmail.com"))
            .ForMember(dest => dest.Email,
                       opt => opt.MapFrom(src => $"{src.PhoneNumber}@gmail.com"));
        }
    }
}
