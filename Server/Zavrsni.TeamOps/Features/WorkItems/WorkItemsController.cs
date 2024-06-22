using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.Features.WorkItems.Commands;
using Zavrsni.TeamOps.Features.WorkItems.Models;
using Zavrsni.TeamOps.Features.WorkItems.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zavrsni.TeamOps.Features.WorkItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public WorkItemsController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] WorkItemCreateModel workItemCreateModel)
        {
            var command = _mapper.Map<CreateWorkItem.Command>(workItemCreateModel);
            var result = await _sender.Send(command);
            return result.GetResponseResult();
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetByProject([FromRoute] Guid projectId)
        {
            var query = new GetWorkItemsByProject.Query { ProjectId = projectId };
            var result = await _sender.Send(query);
            return result.GetResponseResult();
        }

        [HttpGet("user-story/{projectId}")]
        public async Task<IActionResult> GetUserStoriesByProject([FromRoute] Guid projectId)
        {
            var query = new GetUserStoriesByProject.Query { ProjectId = projectId };
            var result = await _sender.Send(query);
            return result.GetResponseResult();
        }

        [HttpGet("by-iteration/{iterationId}")]
        public async Task<IActionResult> GetWorkItemsByIteration([FromRoute] Guid iterationId)
        {
            var query = new GetWorkItemsByIteration.Query { IterationId = iterationId };
            var result = await _sender.Send(query);
            return result.GetResponseResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkItemAsync([FromRoute] Guid id)
        {
            var command = new DeleteWorkItem.Command { Id = id };
            var result = await _sender.Send(command);
            return result.GetResponseResult();
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateWorkItem([FromBody] WorkItemUpdateModel model)
        {
            var command = new UpdateWorkItem.Command {
                Status = model.Status, 
                AssignedToId = model.AssignedToId,
                Description = model.Description, 
                Title = model.Title, 
                Id = model.Id,
                Priority = model.Priority };
            var result = await _sender.Send(command);
            return result.GetResponseResult();
        }
    }
}
