using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.UserDomain.Models;
using Zavrsni.TeamOps.UserDomain.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
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
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
