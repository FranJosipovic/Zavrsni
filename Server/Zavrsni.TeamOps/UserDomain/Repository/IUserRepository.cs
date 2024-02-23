using Zavrsni.TeamOps.UserDomain.Models;

namespace Zavrsni.TeamOps.UserDomain.Repository
{
    public interface IUserRepository
    {
        Task<DbActionResult> PostAsync(UserSignUpModel userSignUpModel);
        Task<DbActionResult> GetAsync(string usernameOrEmail);
    }
}
