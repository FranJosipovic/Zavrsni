using System.Linq.Expressions;
using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.Features.Users.Repository
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetAsync(string usernameOrEmail);
        Task<User> GetAsync(Guid userId);
        Task<bool> UserExists(Guid userId);
        Task<bool> OrganizationHasUser(Guid userId, Guid organizationId);
        Task<bool> IsUnique<T, TValue>(
        Expression<Func<T, TValue>> propertyExpression,
        TValue valueToCheck) where T : class;
    }
}
