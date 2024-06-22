using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Projects.Models;

namespace Zavrsni.TeamOps.Features.Projects.Services
{
    public interface IProjectService
    {
        Task<ServiceActionResult> GetProjectIdByName(string name,Guid organizationId);
        Task<ServiceActionResult> GetByOrganizationId(Guid organizationId);
        Task<ServiceActionResult> CreateNewProject(CreateProjectModel projectModel);
        Task<ServiceActionResult> AddUserToProject(AddUserToProjectModel model);
        Task<ServiceActionResult> GetProjectDetails(Guid projectId);
    }
}
