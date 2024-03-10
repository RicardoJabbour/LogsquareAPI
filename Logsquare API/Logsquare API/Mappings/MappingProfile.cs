using AutoMapper;
using Model.Data.DTOs;
using Model.Data.Models;
using System.Collections.Generic;

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
