using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.OrganizationDomain;
using Zavrsni.TeamOps.OrganizationDomain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zavrsni.TeamOps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationController(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        // GET: api/<OrganizationController>
        [HttpGet]
        public string Get()
        {
            return "Ok";
        }

        // GET api/<OrganizationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrganizationController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationPostModel value)
        {
            var postResult = await _organizationRepository.Post(value);
            return postResult.GetResponseResult();
        }

        // PUT api/<OrganizationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrganizationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
