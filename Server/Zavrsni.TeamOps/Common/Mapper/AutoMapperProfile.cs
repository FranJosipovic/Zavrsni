using AutoMapper;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Iterrations.Commands;
using Zavrsni.TeamOps.Features.Iterrations.Models;
using Zavrsni.TeamOps.Features.Organizations.Models;
using Zavrsni.TeamOps.Features.Projects.Models;
using Zavrsni.TeamOps.Features.ProjectWikis.Commands;
using Zavrsni.TeamOps.Features.ProjectWikis.Models;
using Zavrsni.TeamOps.Features.Users.Commands;
using Zavrsni.TeamOps.Features.Users.Models;
using Zavrsni.TeamOps.Features.Users.Models.DTOs;
using Zavrsni.TeamOps.Features.WorkItems.Commands;
using Zavrsni.TeamOps.Features.WorkItems.Models;

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

            CreateMap<CreateProjectWiki.Command, ProjectWiki>();

            CreateMap<WikiCreateModel, CreateProjectWiki.Command>();

            CreateMap<IterrationCreateModel, CreateIterration.Command>();
            CreateMap<CreateIterration.Command, Iteration>();

            CreateMap<CreateWorkItem.Command, WorkItem>();
            CreateMap<WorkItemCreateModel, CreateWorkItem.Command>();   
        }
    }
}
