using AutoMapper;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Organizations.Repository;
using Zavrsni.TeamOps.Features.Projects.Models;
using Zavrsni.TeamOps.Features.Projects.Repository;
using Zavrsni.TeamOps.Features.Users.Repository;

namespace Zavrsni.TeamOps.Features.Projects.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IOrganizationRepository organizationRepository, IMapper mapper, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _organizationRepository = organizationRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<ServiceActionResult> GetByOrganizationId(Guid organizationId)
        {
            var serviceActionResult = new ServiceActionResult();

            var organizationExists = await _organizationRepository.OrganizationExists(organizationId);
            if (!organizationExists)
            {
                serviceActionResult.SetBadRequest("Organization does not exists");
                return serviceActionResult;
            }
            var projects = await _projectRepository.GetByOrganizationIdAsync(organizationId);
            serviceActionResult.SetOk(new CollectionResponseData<Project>
            {
                Items = projects,
                Count = projects.Count
            }, "Action Successfull");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> CreateNewProject(CreateProjectModel projectModel)
        {
            var serviceActionResult = new ServiceActionResult();

            //check if user is owner of organization and if organization exists
            var userIsOwner = await _organizationRepository.UserIsOwnerOfOrganizationAsync(projectModel.OrganizationOwnerId, projectModel.OrganizationId);
            if (!userIsOwner)
            {
                serviceActionResult.SetBadRequest("User is not owner or organization doesn't exists");
                return serviceActionResult;
            }
            var newProject = _mapper.Map<Project>(projectModel);

            var addedProject = await _projectRepository.AddAsync(newProject);
            await _projectRepository.AddUserToProjectAsync(projectModel.OrganizationOwnerId, addedProject.Id);

            serviceActionResult.SetOk(newProject, "Project successfully created");

            return serviceActionResult;
        }

        public async Task<ServiceActionResult> AddUserToProject(AddUserToProjectModel model)
        {
            var serviceActionResult = new ServiceActionResult();

            var organization = await _organizationRepository.GetByProjectIdAsync(model.ProjectId);

            var userExists = await _userRepository.UserExists(model.UserId);
            var ownerExists = await _userRepository.UserExists(model.OwnerId);
            if (!userExists || !ownerExists)
            {
                serviceActionResult.SetBadRequest("User or owner does not exist");
                return serviceActionResult;
            }

            //check if owner is "real" owner
            var userIsOwnerofOrg = await _organizationRepository.UserIsOwnerOfOrganizationAsync(model.OwnerId, organization.Id);
            if (!userIsOwnerofOrg)
            {
                serviceActionResult.SetBadRequest("user is not owner of organization");
                return serviceActionResult;
            }

            var userIsPartOfProject = await _projectRepository.UserIsPartOfProject(model.UserId, model.ProjectId);
            if (userIsPartOfProject)
            {
                serviceActionResult.SetBadRequest("User is already part of organization");
                return serviceActionResult;
            }
            //add to proj
            await _projectRepository.AddUserToProjectAsync(model.UserId, model.ProjectId);
            //return some object instd of null
            serviceActionResult.SetOk(null, "User successfully added");
            return serviceActionResult;
        }
    }
}
