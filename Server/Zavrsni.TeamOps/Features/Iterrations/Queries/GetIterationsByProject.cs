using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Features.Iterrations.Models.DTOs;
using Zavrsni.TeamOps.Features.WorkItems.Models.DTOs;

namespace Zavrsni.TeamOps.Features.Iterrations.Queries
{
    public class GetIterationsByProject
    {
        public class Query : IRequest<ServiceActionResult>
        {
            public Guid ProjectId { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Query, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;

            public Handler(TeamOpsDbContext db)
            {
                _db = db;
            }

            public async Task<ServiceActionResult> Handle(Query request, CancellationToken cancellationToken)
            {

                try
                {
                    var serviceActionResult = new ServiceActionResult();

                    // Check if the project exists and get the iterations in a single query
                    var projectWithIterations = await _db.Projects
                        .Include(p => p.Iterrations)
                        .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken: cancellationToken);

                    if (projectWithIterations == null)
                    {
                        serviceActionResult.SetBadRequest("Project does not exist");
                        return serviceActionResult;
                    }

                    // Get all work items associated with the iterations in a single query
                    List<IterationDTO> iterations = projectWithIterations.Iterrations.OrderBy(i=>i.OrderNumber).Select(i => new IterationDTO { Number = i.OrderNumber, Id = i.Id }).ToList();

                    serviceActionResult.SetOk(new CollectionResponseData<IterationDTO> { Items = iterations, Count = iterations.Count }, "successfully");
                    return serviceActionResult;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
