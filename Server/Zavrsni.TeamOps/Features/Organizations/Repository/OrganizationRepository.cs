using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Organizations.Models;

namespace Zavrsni.TeamOps.Features.Organizations.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly TeamOpsDbContext _db;
        public OrganizationRepository(TeamOpsDbContext db)
        {
            _db = db;
        }

        public async Task<Guid?> GetIdByNameAsync(string name)
        {
            try
            {
                var id = await _db.Organizations.AsNoTracking().Where(o=>o.Name == name).Select(o=>o.Id).FirstOrDefaultAsync();
                return id == Guid.Empty ? null : id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddUserToOrganizationAsync(Guid userId, Guid organizationId)
        {
            try
            {
                var organization = await _db.Organizations.Include(o => o.Users).FirstAsync(o => o.Id == organizationId);
                var user = await _db.Users.FirstAsync(u => u.Id == userId);
                organization.Users.Add(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddUserToOrganizationAsync(User user, Guid organizationId)
        {
            try
            {
                var organization = await _db.Organizations.Include(o => o.Users).FirstAsync(o => o.Id == organizationId);
                organization.Users.Add(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddAsync(Organization organization)
        {
            try
            {
                _db.Organizations.Add(organization);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Organization> GetWithRelatedAsync(Guid organizationId)
        {
            try
            {
                var organization = await _db.Organizations
                    .Include(o => o.Projects).ThenInclude(p => p.Users)
                    .Include(o => o.Users)
                    .Include(o => o.Owner)
                    .AsNoTracking()
                    .Where(o => o.Id == organizationId)
                    .FirstOrDefaultAsync() ?? throw new ObjectNotFoundException("Organization not found");

                return organization;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Organization> GetAsync(Guid organizationId)
        {
            try
            {
                var organization = await _db.Organizations.AsNoTracking().Where(o => o.Id == organizationId).FirstOrDefaultAsync();
                if (organization is null)
                {
                    throw new ObjectNotFoundException("Organization not found");
                }
                return organization;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IList<Organization>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                var organizations = await _db.Organizations.AsNoTracking().Where(o => o.OwnerId == userId).ToListAsync();
                return organizations;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<Organization>> GetWithUserIdAsync(Guid userId)
        {
            try
            {
                var organizations = await _db.Organizations
                    .Include(o => o.Owner)
                    .Include(o => o.Projects.Where(p => p.Users.Any(u => u.Id == userId)))
                    .AsNoTracking()
                    .Where(o => o.Users.Any(u => u.Id == userId))
                    .ToListAsync();
                return organizations;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Organization> UpdateAsync(UpdateOrganizationDTO updateOrganizationDTO)
        {
            try
            {
                var organization = await _db.Organizations.FindAsync(updateOrganizationDTO.Id)
                    ?? throw new ObjectNotFoundException("Organization not found");

                organization.Description = updateOrganizationDTO.Description;
                organization.Name = updateOrganizationDTO.Name;
                await _db.SaveChangesAsync();
                return organization;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Organization> ChangeNameAsync(Guid organizationId, string newName)
        {
            try
            {
                var organization = await _db.Organizations.FindAsync(organizationId);
                if (organization is null)
                {
                    throw new ObjectNotFoundException("Organization not found");
                }
                organization.Name = newName;
                await _db.SaveChangesAsync();
                return organization;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveAsync(Guid organizationId)
        {
            try
            {
                var organization = await _db.Organizations.FindAsync(organizationId);
                if (organization is null)
                {
                    throw new ObjectNotFoundException("Organization not found");
                }
                _db.Organizations.Remove(organization);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> OrganizationExists(Guid organizationId)
        {
            try
            {
                return await _db.Organizations.AsNoTracking().AnyAsync(u => u.Id == organizationId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UserIsOwnerOfOrganizationAsync(Guid userId, Guid organizationId)
        {
            try
            {
                return await _db.Organizations.AnyAsync(o => o.Id == organizationId && o.OwnerId == userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Organization> GetByProjectIdAsync(Guid projectId)
        {
            try
            {
                var organizationId = await _db.Projects.AsNoTracking()
                    .Where(p => p.Id == projectId)
                    .Select(p => p.OrganizationId)
                    .FirstOrDefaultAsync();
                var organization = await _db.Organizations.FindAsync(organizationId);
                if (organization is null)
                {
                    throw new ObjectNotFoundException("Couldnt find organization by project");
                }
                return organization;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
