using Zavrsni.TeamOps.OrganizationDomain.Models;

namespace Zavrsni.TeamOps.OrganizationDomain
{
    public interface IOrganizationRepository
    {
        Task<DbActionResult> Post(OrganizationPostModel organizationPostModel);
    }
}
