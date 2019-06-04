using AutoMapper;
using SkeletonApi.Api.Model;
using SkeletonApi.Entity;

namespace SkeletonApi.Api.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
