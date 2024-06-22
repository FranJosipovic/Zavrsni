using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.Features.ProjectWikis.Commands;
using Zavrsni.TeamOps.Features.ProjectWikis.Models;
using Zavrsni.TeamOps.Features.ProjectWikis.Queries;

namespace Zavrsni.TeamOps.Features.ProjectWikis
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectWikisController : ControllerBase
    {
        private readonly ISender _sernder;
        private readonly IMapper _mapper;
        public ProjectWikisController(ISender sernder, IMapper mapper)
        {
            _sernder = sernder;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] WikiCreateModel wikiCreateModel)
        {
            var command = _mapper.Map<CreateProjectWiki.Command>(wikiCreateModel);
            var result = await _sernder.Send(command);
            return result.GetResponseResult();
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetByProject([FromRoute] Guid projectId)
        {
            var query = new GetWikisByProject.Querry { ProjectId = projectId };
            var result = await _sernder.Send(query);
            return result.GetResponseResult();
        }

        [HttpGet("wikis/{wikiId}")]
        public async Task<IActionResult> GetData([FromRoute] Guid wikiId)
        {
            var query = new GetWikiData.Querry { ProjectWikiId = wikiId };
            var result = await _sernder.Send(query);
            return result.GetResponseResult();
        }

        [HttpPut("wikis/{wikiId}")]
        public async Task<IActionResult> UpdateWiki([FromRoute] Guid wikiId, [FromBody] UpdateWikiContentRequest content)
        {
            var command = new UpdateProjectWiki.Command { Id = wikiId, Content = content.Content, Title = content.Title };
            var result = await _sernder.Send(command);
            return result.GetResponseResult();
        }

        [HttpDelete("{wikiId}")]
        public async Task<IActionResult> DeleteWiki([FromRoute] Guid wikiId)
        {
            var command = new DeleteProjectWiki.Command { Id = wikiId };
            var result = await _sernder.Send(command);
            return result.GetResponseResult();
        }
    }
}

