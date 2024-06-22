using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity;

namespace Zavrsni.TeamOps.Features.ProjectWikis.Commands
{
    public class UpdateProjectWiki
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public Guid Id { get; set; }
            public string Content { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
        }

        internal sealed class Handler : IRequestHandler<Command, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;
            public Handler(TeamOpsDbContext db)
            {
                _db = db;
            }

            public async Task<ServiceActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new ServiceActionResult();

                    var projectWiki = await _db.ProjectWikis.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);

                    if(projectWiki is null)
                    {
                        result.SetNotFound("Couldn't find item");
                    }

                    projectWiki!.Title = request.Title;
                    projectWiki.Content = request.Content;
                    projectWiki.UpdatedOn = DateTime.Now;

                    await _db.SaveChangesAsync(cancellationToken);
                    result.SetOk(projectWiki, "Updated successfully");
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
