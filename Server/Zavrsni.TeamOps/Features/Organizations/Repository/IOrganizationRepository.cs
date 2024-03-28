using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Organizations.Models;

namespace Zavrsni.TeamOps.Features.Organizations.Repository
{
    public interface IOrganizationRepository
    {
        Task AddUserToOrganizationAsync(Guid userId, Guid organizationId);
        Task AddUserToOrganizationAsync(User user, Guid organizationId);
        Task AddAsync(Organization organization);
        Task<Organization> GetWithRelatedAsync(Guid organizationId);
        Task<Organization> GetAsync(Guid organizationId);
        Task<IList<Organization>> GetByUserIdAsync(Guid userId);
        Task<IList<Organization>> GetWithUserIdAsync(Guid userId);
        Task<Organization> UpdateAsync(UpdateOrganizationDTO updateOrganizationDTO);
        Task<Organization> ChangeNameAsync(Guid organizationId, string newName);
        Task RemoveAsync(Guid organizationId);
        Task<bool> OrganizationExists(Guid organizationId);
        Task<bool> UserIsOwnerOfOrganizationAsync(Guid userId, Guid organizationId);
        Task<Organization> GetByProjectIdAsync(Guid projectId);
        Task<Guid?> GetIdByNameAsync(string name);
    }
}
