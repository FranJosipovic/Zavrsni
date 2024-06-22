using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity;

namespace Zavrsni.TeamOps.Features.ProjectWikis.Commands
{
    public class DeleteProjectWiki
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;
            public Handler(TeamOpsDbContext db)
            {
                _db = db;
            }

            public List<ProjectWiki> ListToDelete { get; set; } = new List<ProjectWiki>();

            public async Task<ServiceActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new ServiceActionResult();

                    var wiki = await _db.ProjectWikis.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
                    if (wiki is null)
                    {
                        result.SetNotFound("Cannot find wiki");
                        return result;
                    }
                    ListToDelete.Add(wiki);
                    await AccumulateAllChildren(wiki.Id);

                    _db.ProjectWikis.RemoveRange(ListToDelete);
                    await _db.SaveChangesAsync(cancellationToken);
                    result.SetOk(request.Id, "Successfully deleted");
                    return result;

                }
                catch (Exception)
                {
                    throw;
                }
            }

            private async Task AccumulateAllChildren(Guid parentId)
            {
                var children = _db.ProjectWikis.Where(wiki => wiki.ParentId == parentId).ToList();
                foreach (var item in children)
                {
                    await AccumulateAllChildren(item.Id);
                }
                if (children.Count > 0)
                {
                    ListToDelete.AddRange(children);
                }
            }
        }
    }
}
