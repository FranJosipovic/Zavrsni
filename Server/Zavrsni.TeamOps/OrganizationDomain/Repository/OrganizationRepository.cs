﻿using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.OrganizationDomain.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly TeamOpsDbContext _db;
        public OrganizationRepository(TeamOpsDbContext db)
        {
            _db = db;
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
                if(organization is null)
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

    }
}
