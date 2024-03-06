using AutoMapper;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Organizations.Models;
using Zavrsni.TeamOps.Features.Projects.Models;
using Zavrsni.TeamOps.Features.Users.Commands;
using Zavrsni.TeamOps.Features.Users.Models;

namespace Zavrsni.TeamOps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() { 
            CreateMap<UserSignUpModel,User>().ReverseMap();
            CreateMap<OrganizationPostModel,Organization>().ReverseMap();
            CreateMap<User, UserNoSensitiveInfoDTO>();
            CreateMap<CreateProjectModel, Project>();

            CreateMap<UserSignUpModel, UserSignUp.Command>().ReverseMap();
            CreateMap<User, UserSignUp.Command>().ReverseMap();

            CreateMap<UserSignInModel, UserSignIn.Command>().ReverseMap();
        }
    }
}
