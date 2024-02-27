using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.OrganizationDomain.Models;

namespace Zavrsni.TeamOps.OrganizationDomain.Service
{
    public interface IOrganizationService
    {
        Task<ServiceActionResult> RemoveOrganization(Guid organizationId);
        Task<ServiceActionResult> ChangeOrganizationName(Guid organizationId, string newName);
        Task<ServiceActionResult> GetOrganizationsByOwnerId(Guid ownerId);
        Task<ServiceActionResult> CreateOrganization(OrganizationPostModel organizationPostModel);
        Task<ServiceActionResult> AddUser(Guid userId, Guid organizationId);
    }
}
