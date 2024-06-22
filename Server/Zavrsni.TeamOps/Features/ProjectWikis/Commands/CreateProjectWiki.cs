using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity;

namespace Zavrsni.TeamOps.Features.ProjectWikis.Commands
{
    public class CreateProjectWiki
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public string Title { get; set; }
            public string Content { get; set; } = string.Empty;
            public Guid ProjectId { get; set; }
            public Guid CreatedById { get; set; }
            public Guid? ParentId { get; set; }
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

                    //user exists
                    var userExists = await _db.Users.AnyAsync(p => p.Id == request.CreatedById, cancellationToken);
                    if (!userExists)
                    {
                        result.SetBadRequest("User does not exists");
                        return result;
                    }

                    //project exists
                    var projectExists = await _db.Projects.AnyAsync(p => p.Id == request.ProjectId, cancellationToken);
                    if (!projectExists)
                    {
                        result.SetBadRequest("Project does not exists");
                    }

                    var newEntity = _mapper.Map<ProjectWiki>(request);
                    newEntity.UpdatedOn = DateTime.Now;
                    newEntity.CreatedOn = DateTime.Now;

                    await _db.ProjectWikis.AddAsync(newEntity);
                    await _db.SaveChangesAsync();
                    result.SetResultCreated(true);
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
