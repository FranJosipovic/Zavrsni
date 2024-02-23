using AutoMapper;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.UserDomain.Models;

namespace Zavrsni.TeamOps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() { 
            CreateMap<UserSignUpModel,User>().ReverseMap();
        }
    }
}
