using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using System.Linq.Expressions;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.UserDomain.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TeamOpsDbContext _db;

        public UserRepository(TeamOpsDbContext db, IMapper mapper)
        {
            _db = db;
        }

        public async Task AddAsync(User user)
        {
            try
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetAsync(string usernameOrEmail)
        {
            try
            {
                var user = await _db.Users.Where(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail).AsNoTracking().FirstOrDefaultAsync();
                if (user is null)
                {
                    throw new ObjectNotFoundException("User with email or username not found");
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetAsync(Guid userId)
        {
            try
            {
                var user = await _db.Users.AsNoTracking().Where(u => u.Id == userId).FirstOrDefaultAsync();
                if (user is null)
                {
                    throw new ObjectNotFoundException("User not found");
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UserExists(Guid userId)
        {
            try
            {
                return await _db.Users.AsNoTracking().AnyAsync(u => u.Id == userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> OrganizationHasUser(Guid userId, Guid organizationId)
        {
            return await _db.Organizations.AsNoTracking().AnyAsync(o => o.Id == organizationId && o.Users.Any(u => u.Id == userId));
        }

        public async Task<bool> IsUnique<T, TValue>(
        Expression<Func<T, TValue>> propertyExpression,
        TValue valueToCheck) where T : class
        {
            // Prepare the lambda expression for AnyAsync
            var parameter = Expression.Parameter(typeof(T), "entity");
            var body = Expression.Equal(propertyExpression.Body, Expression.Constant(valueToCheck));
            var lambda = Expression.Lambda<Func<T, bool>>(body, propertyExpression.Parameters);

            // Use the prepared lambda in AnyAsync to check for uniqueness
            return !await _db.Set<T>().AnyAsync(lambda);
        }
    }
}
