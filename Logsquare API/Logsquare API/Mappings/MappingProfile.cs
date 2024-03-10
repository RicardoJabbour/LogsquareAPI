using AutoMapper;
using Model.Data.DTOs;
using Model.Data.Models;

namespace Logsquare_API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
