using AutoMapper;
using CRMSystem.Application.Dtos.Notification;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.MappingProfiles
{
    public class AdminNotificationProfile : Profile
    {
        public AdminNotificationProfile()
        {
            CreateMap<AdminNotification, AdminNotificationDto>()
                // DateTime → ISO-8601 formatlı string
                .ForMember(
                    dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate.ToString(("dd.MM.yyyy HH:mm:ss")))
                );
        }
    }
}
