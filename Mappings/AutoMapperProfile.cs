using AutoMapper;
using TaskFlow.DTOs;
using TaskFlow.Models;

namespace TaskFlow.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entity -> DTO
            CreateMap<User, LoginResponseDto>();
            // Entity -> DTO
            CreateMap<User, RegisterResponseDto>();

            // DTO -> Entity (for registration)
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // we hash password manually
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
