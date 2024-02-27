using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.UserDomain.Models;

namespace Zavrsni.TeamOps.UserDomain.Service
{
    public interface IUserService
    {
        Task<ServiceActionResult> SignUp(UserSignUpModel userSignUpModel);
        Task<ServiceActionResult> SignIn(UserSignInModel signInModel);
    }
}
