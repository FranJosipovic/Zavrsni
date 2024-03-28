using Zavrsni.TeamOps.Features.Organizations.Models;

namespace Zavrsni.TeamOps.Features.Organizations.Service
{
    public interface IOrganizationService
    {
        Task<ServiceActionResult> GetOrganizationIdByName(string name);
        Task<ServiceActionResult> Get(Guid organizationId);
        Task<ServiceActionResult> RemoveOrganization(Guid organizationId);
        Task<ServiceActionResult> UpdateOrganization(UpdateOrganizationDTO updateOrganizationDTO);
        Task<ServiceActionResult> ChangeOrganizationName(Guid organizationId, string newName);
        Task<ServiceActionResult> GetOrganizationsByOwnerId(Guid ownerId);
        Task<ServiceActionResult> GetOrganizationsWithUserId(Guid userId);
        Task<ServiceActionResult> CreateOrganization(OrganizationPostModel organizationPostModel);
        Task<ServiceActionResult> AddUser(Guid userId, Guid organizationId);
    }
}
