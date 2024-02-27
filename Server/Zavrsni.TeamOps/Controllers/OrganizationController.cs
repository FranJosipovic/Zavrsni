using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.OrganizationDomain.Models;
using Zavrsni.TeamOps.OrganizationDomain.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zavrsni.TeamOps.Controllers
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

        // GET api/<OrganizationController>/5
        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwnerId(Guid ownerId)
        {
            var result = await _organizationService.GetOrganizationsByOwnerId(ownerId);
            return result.GetResponseResult();
        }

        // POST api/<OrganizationController>
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
        // PUT api/<OrganizationController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeNameAsync(Guid id, [FromBody] string newName)
        {
            var result = await _organizationService.ChangeOrganizationName(id, newName);
            return result.GetResponseResult();
        }

        // DELETE api/<OrganizationController>/5
        [HttpDelete("{organizationId}")]
        public async Task<IActionResult> DeleteAsync(Guid organizationId)
        {
            var result = await _organizationService.RemoveOrganization(organizationId);
            return result.GetResponseResult();
        }
    }
}
