using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.UserDomain.Models;
using Zavrsni.TeamOps.UserDomain.Service;

namespace Zavrsni.TeamOps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpModel userSignUpModel)
        {
            var result = await _userService.SignUp(userSignUpModel);
            return result.GetResponseResult();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInModel userSignInModel)
        {
            var result = await _userService.SignIn(userSignInModel);
            return result.GetResponseResult();
        }
    }
}
