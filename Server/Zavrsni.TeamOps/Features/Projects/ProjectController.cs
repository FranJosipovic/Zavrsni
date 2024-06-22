using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.Features.Projects.Models;
using Zavrsni.TeamOps.Features.Projects.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zavrsni.TeamOps.Features.Projects
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

        [HttpGet("{name}/projectId/{organizationId}")]
        public async Task<IActionResult> GetIdByName(string name, Guid organizationId)
        {
            var result = await _projectService.GetProjectIdByName(name, organizationId);
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

        [HttpGet("details/{projectId}")]
        public async Task<IActionResult> GetDetails([FromRoute] Guid projectId)
        {
            var result = await _projectService.GetProjectDetails(projectId);
            return result.GetResponseResult();
        }
    }
}
