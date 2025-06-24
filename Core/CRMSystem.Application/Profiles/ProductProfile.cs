using AutoMapper;
using CRMSystem.Application.Dtos.Product;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // CreateProductDto → Product (we handle the IFormFile upload in the service)
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImage, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore());

            // UpdateProductDto → Product
            CreateMap<UpdatePendingProductDto, Product>()
         
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore());

            // Product → ProductDto (read model)
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId.ToString()))

                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Measure, opt => opt.MapFrom(src => src.Measure))
                .ForMember(dest => dest.IsCreated, opt => opt.MapFrom(src => src.IsCreated))
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.Deleted))
                .ForMember(dest => dest.IsUpdated, opt => opt.MapFrom(src => src.IsUpdated));

            // PendingProduct → PendingProductDetailsDto
            CreateMap<PendingProduct, PendingProductDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.OldName, opt => opt.MapFrom(src => src.OriginalProduct!.Name))
                .ForMember(dest => dest.OldMeasure, opt => opt.MapFrom(src => src.OriginalProduct!.Measure))
                .ForMember(dest => dest.NewName, opt => opt.MapFrom(src => src.NewName))
                .ForMember(dest => dest.NewMeasure, opt => opt.MapFrom(src => src.NewMeasure))
                .ForMember(dest => dest.IsUpdated, opt => opt.MapFrom(src => src.OriginalProduct!.IsUpdated));
        }
    }
}
