using AutoMapper;
using MediatR;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.EF.Enums;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Iterrations.Commands;
using Zavrsni.TeamOps.Features.Organizations.Repository;
using Zavrsni.TeamOps.Features.Projects.Models;
using Zavrsni.TeamOps.Features.Projects.Repository;
using Zavrsni.TeamOps.Features.Users.Repository;
using Zavrsni.TeamOps.Features.WorkItems.Models.DTOs;
using Zavrsni.TeamOps.Features.WorkItems.Queries;

namespace Zavrsni.TeamOps.Features.Projects.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISender _sender;

        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IOrganizationRepository organizationRepository, IMapper mapper, IUserRepository userRepository, ISender sender)
        {
            _projectRepository = projectRepository;
            _organizationRepository = organizationRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _sender = sender;
        }

        public async Task<ServiceActionResult> GetProjectIdByName(string name, Guid organizationId)
        {
            ServiceActionResult serviceActionResult = new ServiceActionResult();
            var id = await _projectRepository.GetIdByNameAsync(name, organizationId);
            if (id == null)
            {
                serviceActionResult.SetNotFound($"Project {name} with provided organizationId does not exists");
                return serviceActionResult;
            }
            serviceActionResult.SetOk(new { Id = id }, "Action Successfull");
            return serviceActionResult;
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

            //create default iteration for project
            var command = new CreateIterration.Command { ProjectId = addedProject.Id, StartsAt = DateTime.Now, EndsAt = DateTime.Now.AddDays(14) };
            await _sender.Send(command);

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
            //return some object instead of null
            serviceActionResult.SetOk(null, "User successfully added");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> GetProjectDetails(Guid projectId)
        {
            var serviceActionResult = new ServiceActionResult();

            var project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                serviceActionResult.SetBadRequest("Project does not exist");
                return serviceActionResult;
            }
            var workItemsResponse = await _sender.Send(new GetWorkItemsByProject.Query { ProjectId = project.Id });
            var workItems = (CollectionResponseData<WorkItemResponseItemDTO>)workItemsResponse.GetData()!;
            List<WorkItemResponseItemDTO> userStories = new List<WorkItemResponseItemDTO>();
            List<WorkItemResponseItemDTO> tasks_bugs = new List<WorkItemResponseItemDTO>();
            foreach (var workItem in workItems.Items)
            {
                if (workItem.Type == WorkItemType.User_Story.ToString().Replace('_',' '))
                {
                    userStories.Add(workItem);
                }
                else
                {
                    tasks_bugs.Add(workItem);
                }
            }

            var iterationsCount = await _projectRepository.GetIterationsCountByProjectAsync(project.Id);
            var usersCount = await _projectRepository.GetUsersCountByProjectAsync(project.Id);

            serviceActionResult.SetOk(new ProjectDetailsDTO
            {
                Title = project.Name,
                UserStories = userStories.Count,
                Task_bugs = tasks_bugs.Count,
                Iterations = iterationsCount,
                Users = usersCount
            },"success");
            return serviceActionResult;
        }
    }
}
