using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zavrsni.TeamOps.Features.Iterrations.Commands;
using Zavrsni.TeamOps.Features.Iterrations.Models;
using Zavrsni.TeamOps.Features.Iterrations.Queries;

namespace Zavrsni.TeamOps.Features.Iterrations
{
    [Route("api/[controller]")]
    [ApiController]
    public class IterationsController : ControllerBase
    {
        private readonly ISender _sernder;
        private readonly IMapper _mapper;
        public IterationsController(ISender sernder, IMapper mapper)
        {
            _sernder = sernder;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] IterrationCreateModel iterrationCreateModel)
        {
            var command = _mapper.Map<CreateIterration.Command>(iterrationCreateModel);
            var result = await _sernder.Send(command);
            return result.GetResponseResult();
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetByProject([FromRoute] Guid projectId)
        {
            var query = new GetIterationsByProject.Query { ProjectId = projectId };
            var result = await _sernder.Send(query);
            return result.GetResponseResult();
        }
    }
}

