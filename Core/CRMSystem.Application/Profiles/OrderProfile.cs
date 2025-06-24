using AutoMapper;
using CRMSystem.Application.Dtos.OrderCustomer;
using CRMSystem.Application.Dtos.OrderItem;
using CRMSystem.Application.Dtos.OrderOverheadDtos;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.Dtos.Section;
using CRMSystem.Domain.Entities;
using CRMSystem.Application.Dtos.OrderItemCustomer;

namespace CRMSystem.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // CreateOrderDto → Order
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => Guid.Parse(src.SectionId)))
        
                .ForMember(dest => dest.OrderLimitTime, opt => opt.MapFrom(src => DateTime.ParseExact(src.OrderLimitTime, "dd.MM.yyyy", null)))
                .ForMember(dest => dest.Items, opt => opt.Ignore()) // Handled manually
                .ForMember(dest => dest.Overhead, opt => opt.Ignore());

            // Order → OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.OrderLimitTime, opt => opt.MapFrom(src => src.OrderLimitTime.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.OrderDeliveryTime, opt => opt.MapFrom(src => src.OrderDeliveryTime.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.AdminInfo, opt => opt.MapFrom(src => src.Admin))
                .ForMember(dest => dest.FighterInfo, opt => opt.MapFrom(src => src.Fighter))
                .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.Section))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.OverheadNames, opt => opt.MapFrom(src => src.Overhead.Select(o => o.FileName).ToList()));

      

            // Section → SectionDto (əgər artıq yoxdursa)
            CreateMap<Section, SectionDto>();

            // OrderItem → OrderItemDto
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId.ToString()))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : ""))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor != null ? src.Vendor.Name : ""))
                .ForMember(dest => dest.OrderItemDeliveryTime, opt => opt.MapFrom(src => src.OrderItemDeliveryTime.ToString("dd.MM.yyyy")));

            // OrderOverhead → CreateOrderOverheadDto & vice versa
            CreateMap<CreateOrderOverheadDto, OrderOverhead>().ReverseMap();
    

            // CreateOrderItemDto → OrderItem
            CreateMap<CreateOrderItemDto, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => Guid.Parse(src.ProductId)))
                .ForMember(dest => dest.RequiredQuantity, opt => opt.MapFrom(src => src.RequiredQuantity));
        }
    }
}
