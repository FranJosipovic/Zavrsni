using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.Common.Connectors;
using Zavrsni.TeamOps.Features.Users.Commands;
using Zavrsni.TeamOps.Features.Users.Models;
using Zavrsni.TeamOps.Features.Users.Queries;

namespace Zavrsni.TeamOps.Features.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sernder;
        private readonly IMapper _mapper;
        private readonly IGithubConnector _githubConnector;
        public UserController(ISender sernder, IMapper mapper, IGithubConnector githubConnector)
        {
            _sernder = sernder;
            _mapper = mapper;
            _githubConnector = githubConnector;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpModel userSignUpModel)
        {
            var command = _mapper.Map<UserSignUp.Command>(userSignUpModel);
            var result = await _sernder.Send(command);
            return result.GetResponseResult();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInModel userSignInModel)
        {
            var command = _mapper.Map<UserSignIn.Command>(userSignInModel);
            var result = await _sernder.Send(command);
            return result.GetResponseResult();
        }

        [HttpGet("by-organization/{organizationId}")]
        public async Task<IActionResult> GetByOrganization([FromRoute] Guid organizationId)
        {
            var querry = new GetUsersByOrganization.Querry { OrganizationId = organizationId };
            var result = await _sernder.Send(querry);
            return result.GetResponseResult();
        }

        [HttpGet("by-project/{projectId}")]
        public async Task<IActionResult> GetByProject([FromRoute] Guid projectId)
        {
            var querry = new GetUsersByProject.Querry { ProjectId = projectId };
            var result = await _sernder.Send(querry);
            return result.GetResponseResult();
        }
        [HttpGet("user/github-membership-status/{userName}")]
        public async Task<IActionResult> GetUserMembershipStatus([FromRoute] string userName)
        {
            var querry = new GetUserMembershipStatus.Querry { GithubUser = userName };
            var result = await _sernder.Send(querry);
            return result.GetResponseResult();
        }

        [HttpGet("user/github/{q}")]
        public async Task<IActionResult> SearchGithubUser([FromRoute] string q)
        {
            var querry = new SearchGithubUsers.Querry { q = q };
            var result = await _sernder.Send(querry);
            return result.GetResponseResult();
        }

        [HttpGet("user/{q}")]
        public async Task<IActionResult> SearchUsers([FromRoute] string q)
        {
            var querry = new SearchUsers.Querry { q = q };
            var result = await _sernder.Send(querry);
            return result.GetResponseResult();
        }
    }
}
