using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.ProjectDomain.Models;
using Zavrsni.TeamOps.ProjectDomain.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zavrsni.TeamOps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpGet("projects/organization/{organizationId}")]
        public async Task<IActionResult> GetByOrganizationAsync([FromRoute] Guid organizationId)
        {
            var result = await _projectService.GetByOrganizationId(organizationId);
            return result.GetResponseResult();
        }

        [HttpPost("user-to-project")]
        public async Task<IActionResult> AddUserToProjectAsync([FromBody] AddUserToProjectModel model)
        {
            var result = await _projectService.AddUserToProject(model);
            return result.GetResponseResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectModel project)
        {
            var result = await _projectService.CreateNewProject(project);
            return result.GetResponseResult();
        }
    }
}
