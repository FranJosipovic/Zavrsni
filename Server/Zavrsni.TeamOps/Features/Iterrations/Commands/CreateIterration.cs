using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity;

namespace Zavrsni.TeamOps.Features.Iterrations.Commands
{
    public class CreateIterration
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public Guid ProjectId { get; set; }
            public DateTime StartsAt { get; set; }
            public DateTime EndsAt { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;
            private readonly IMapper _mapper;
            public Handler(TeamOpsDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<ServiceActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new ServiceActionResult();

                    //check if project exists
                    var projectExists = await _db.Projects.AnyAsync(p => p.Id == request.ProjectId);
                    if (!projectExists)
                    {
                        result.SetBadRequest("Project does not exists");
                        return result;
                    }

                    var latestIteration = await _db.Iterations.Where(i => i.ProjectId == request.ProjectId).OrderByDescending(i => i.OrderNumber).FirstOrDefaultAsync();

                    var newIteration = _mapper.Map<Iteration>(request);

                    if(latestIteration == null) 
                    {
                        newIteration.OrderNumber = 0;
                    }
                    else
                    {
                        newIteration.OrderNumber = latestIteration.OrderNumber + 1;
                    }

                    await _db.Iterations.AddAsync(newIteration, cancellationToken);
                    await _db.SaveChangesAsync(cancellationToken);
                    result.SetOk(newIteration.Id, "Iteration successfully created");

                    return result;

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
