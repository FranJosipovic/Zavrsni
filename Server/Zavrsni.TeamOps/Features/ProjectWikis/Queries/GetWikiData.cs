using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Permissions;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity;

namespace Zavrsni.TeamOps.Features.ProjectWikis.Queries
{
    public class GetWikiData
    {
        public class Querry : IRequest<ServiceActionResult>
        {
            public Guid ProjectWikiId { get; set; }
        }
        public class ProjectWikiItem
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
        }
        internal sealed class Handler : IRequestHandler<Querry, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;
            public Handler(TeamOpsDbContext db)
            {
                _db = db;
            }

            public async Task<ServiceActionResult> Handle(Querry request, CancellationToken cancellationToken)
            {
                try
                {
                    var serviceActionResult = new ServiceActionResult();

                    var data = await _db.ProjectWikis
                        .AsNoTracking()
                        .Include(c => c.CreatedBy)
                        .Where(o => o.Id == request.ProjectWikiId)
                        .Select(s => new ProjectWikiItem
                        {
                            Id = s.Id,
                            Title = s.Title,
                            Content = s.Content,
                            CreatedOn = s.CreatedOn,
                            CreatedBy = s.CreatedBy.Name + s.CreatedBy.Surname
                        }).FirstOrDefaultAsync();

                    if (data == null)
                    {
                        serviceActionResult.SetNotFound("Wiki not found");
                        return serviceActionResult;
                    }

                    serviceActionResult.SetOk(data, "Action successfull");

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
