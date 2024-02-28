using Zavrsni.TeamOps.ProjectDomain.Models;

namespace Zavrsni.TeamOps.ProjectDomain.Services
{
    public interface IProjectService
    {
        Task<ServiceActionResult> GetByOrganizationId(Guid organizationId);
        Task<ServiceActionResult> CreateNewProject(CreateProjectModel projectModel);
        Task<ServiceActionResult> AddUserToProject(AddUserToProjectModel model);
    }
}
