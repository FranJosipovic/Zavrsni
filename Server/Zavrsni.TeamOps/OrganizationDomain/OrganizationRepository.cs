using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.OrganizationDomain.Models;

namespace Zavrsni.TeamOps.OrganizationDomain
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly TeamOpsDbContext _db;
        public OrganizationRepository(TeamOpsDbContext db)
        {
            _db = db;
        }

        public async Task<DbActionResult> Post(OrganizationPostModel organizationPostModel)
        {
            var result = new DbActionResult();
            try
            {
                var organization = new Entity.Models.Organization { Name = organizationPostModel.Name };
                _db.Organizations.Add(organization);
                await _db.SaveChangesAsync();
                result.SetResultCreated(organization);
                return result;
            }
            catch (Exception)
            {
                result.SetInternalError();
                return result;
            }
        }
    }
}
