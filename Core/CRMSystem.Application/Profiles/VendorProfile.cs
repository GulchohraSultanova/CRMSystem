using AutoMapper;
using CRMSystem.Application.Dtos.Vendor;
using CRMSystem.Domain.Entities;
using System.Linq;

namespace CRMSystem.Application.Profiles
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            // Entity → DTO
            CreateMap<Vendor, VendorDto>()
                .ForMember(dest => dest.TotalSale, opt => opt.MapFrom(src
                    => src.OrderItems
                          .Where(oi => oi.VendorId == src.Id
                                       && oi.Order != null
                                       && oi.Order.FighterConfirm)
                          .Sum(oi => oi.Price ?? 0m)
                ));

            // DTO → Entity
            CreateMap<CreateVendorDto, Vendor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            CreateMap<UpdateVendorDto, Vendor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        }
    }
}
