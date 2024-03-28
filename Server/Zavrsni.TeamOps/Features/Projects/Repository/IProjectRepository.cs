using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.Features.Projects.Repository
{
    public interface IProjectRepository
    {
        Task<Project> AddAsync(Project project);
        Task<bool> UserIsPartOfProject(Guid userId, Guid projectId);
        Task AddUserToProjectAsync(Guid userId, Guid projectId);
        Task<IList<Project>> GetByOrganizationIdAsync(Guid organizationId);
        Task<Guid?> GetIdByNameAsync(string name, Guid organizationId);
    }
}
