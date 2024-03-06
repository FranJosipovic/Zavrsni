using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.Features.Projects.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TeamOpsDbContext _db;

        public ProjectRepository(TeamOpsDbContext db)
        {
            _db = db;
        }

        public async Task<Project> AddAsync(Project project)
        {
            try
            {
                var addedProjectResult = await _db.Projects.AddAsync(project);
                await _db.SaveChangesAsync();
                return addedProjectResult.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UserIsPartOfProject(Guid userId, Guid projectId)
        {
            try
            {
                return await _db.Projects.Include(p => p.Users).AnyAsync(p => p.Id == projectId && p.Users.Any(u => u.Id == userId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddUserToProjectAsync(Guid userId, Guid projectId)
        {
            try
            {
                var project = await _db.Projects.Include(o => o.Users).FirstAsync(o => o.Id == projectId);
                if (project is null) throw new ObjectNotFoundException("Couldn't find project");
                var user = await _db.Users.FirstAsync(u => u.Id == userId);
                if (user is null) throw new ObjectNotFoundException("Couldn't find user");
                project.Users.Add(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<Project>> GetByOrganizationIdAsync(Guid organizationId)
        {
            try
            {
                return await _db.Projects.AsNoTracking().Where(p => p.OrganizationId == organizationId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
