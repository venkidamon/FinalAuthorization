using Authorization.Dtos;
using Authorization.Entities;
using AutoMapper;

namespace Authorization.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>();
            CreateMap<RegisterDto, AppUser>();
        }
    }
}
