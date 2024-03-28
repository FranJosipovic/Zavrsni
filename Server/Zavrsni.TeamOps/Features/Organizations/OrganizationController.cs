using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.Features.Organizations.Models;
using Zavrsni.TeamOps.Features.Organizations.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zavrsni.TeamOps.Features.Organizations
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("{organizationId}")]
        public async Task<IActionResult> Get(Guid organizationId)
        {
            var result = await _organizationService.Get(organizationId);
            return result.GetResponseResult();
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwnerId(Guid ownerId)
        {
            var result = await _organizationService.GetOrganizationsByOwnerId(ownerId);
            return result.GetResponseResult();
        }

        [HttpGet("with-user/{userId}")]
        public async Task<IActionResult> GetWithUserId(Guid userId)
        {
            var result = await _organizationService.GetOrganizationsWithUserId(userId);
            return result.GetResponseResult();
        }

        [HttpGet("{name}/organizationId")]
        public async Task<IActionResult> GetIdByName(string name)
        {
            var result = await _organizationService.GetOrganizationIdByName(name);
            return result.GetResponseResult();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationPostModel value)
        {
            var serviceResult = await _organizationService.CreateOrganization(value);
            return serviceResult.GetResponseResult();
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] AddUserPostModel addUserPostModel)
        {
            var serviceResult = await _organizationService.AddUser(addUserPostModel.UserId, addUserPostModel.OrganizationId);
            return serviceResult.GetResponseResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeNameAsync(Guid id, [FromBody] string newName)
        {
            var result = await _organizationService.ChangeOrganizationName(id, newName);
            return result.GetResponseResult();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateOrganizationDTO updateOrganizationDTO)
        {
            var result = await _organizationService.UpdateOrganization(updateOrganizationDTO);
            return result.GetResponseResult();
        }

        [HttpDelete("{organizationId}")]
        public async Task<IActionResult> DeleteAsync(Guid organizationId)
        {
            var result = await _organizationService.RemoveOrganization(organizationId);
            return result.GetResponseResult();
        }
    }
}
