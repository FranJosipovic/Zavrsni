using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.OrganizationDomain.Repository
{
    public interface IOrganizationRepository
    {
        Task AddUserToOrganizationAsync(Guid userId, Guid organizationId);
        Task AddUserToOrganizationAsync(User user, Guid organizationId);
        Task AddAsync(Organization organization);
        Task<Organization> GetAsync(Guid organizationId);
        Task<IList<Organization>> GetByUserIdAsync(Guid userId);
        Task<Organization> ChangeNameAsync(Guid organizationId, string newName);
        Task RemoveAsync(Guid organizationId);
        Task<bool> OrganizationExists(Guid organizationId);
        Task<bool> UserIsOwnerOfOrganizationAsync(Guid userId, Guid organizationId);
        Task<Organization> GetByProjectIdAsync(Guid projectId);
    }
}
