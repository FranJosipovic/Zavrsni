using Zavrsni.TeamOps.Features.Organizations.Models;

namespace Zavrsni.TeamOps.Features.Organizations.Service
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
