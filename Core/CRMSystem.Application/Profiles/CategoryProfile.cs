using AutoMapper;
using CRMSystem.Application.Dtos.Category;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.IsCreated, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.IsUpdated, opt => opt.Ignore());



            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<PendingCategory, PendingCategoryDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.OldName, opt => opt.MapFrom(src => src.OriginalCategory.Name))
                .ForMember(dest => dest.NewName, opt => opt.MapFrom(src => src.NewName))
                .ForMember(dest => dest.IsUpdated, opt => opt.MapFrom(src => src.OriginalCategory.IsUpdated));

            CreateMap<UpdatePendingCategoryDto, PendingCategory>()
                .ForMember(dest => dest.NewName, opt => opt.MapFrom(src => src.NewName));
        }
    }
}
