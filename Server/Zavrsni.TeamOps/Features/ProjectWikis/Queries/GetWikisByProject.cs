using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity;

namespace Zavrsni.TeamOps.Features.ProjectWikis.Queries
{
    public class GetWikisByProject
    {
        public class Querry : IRequest<ServiceActionResult>
        {
            public Guid ProjectId { get; set; }
        }

        public class WikiCollectionItem
        {
            public string Title { get; set; }
            public Guid? ParentId { get; set; }
            public string Content { get; set; }
            public string CreatedBy { get; set; }
            public List<WikiCollectionItem> Children { get; set; }
            public DateTime CreatedOn { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Querry, ServiceActionResult>
        {
            private readonly TeamOpsDbContext _db;
            private readonly IMapper _mapper;
            public Handler(TeamOpsDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<ServiceActionResult> Handle(Querry request, CancellationToken cancellationToken)
            {
                try
                {
                    var serviceActionResult = new ServiceActionResult();

                    var fullRes = await _db.ProjectWikis.Include(x => x.Children).ToListAsync();
                    var wikis = fullRes.Where(pw => pw.ProjectId == request.ProjectId && pw.ParentId == null).ToList();

                    //var wikis = await _db.ProjectWikis.Where(pw => pw.ProjectId == request.ProjectId && pw.ParentId == null)
                    //    .Select(GetWikiProjection(10, 0)).ToListAsync();

                    serviceActionResult.SetOk(new CollectionResponseData<ProjectWiki>
                    {
                        Items = wikis,
                        Count = wikis.Count
                    }, "Action successfull");

                    return serviceActionResult;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static Expression<Func<ProjectWiki, WikiCollectionItem>> GetWikiProjection(int maxDepth, int currentDepth = 0)
            {
                currentDepth++;

                Expression<Func<ProjectWiki, WikiCollectionItem>> result = wiki => new WikiCollectionItem()
                {
                    Content = wiki.Content,
                    Title = wiki.Title,
                    CreatedBy = wiki.CreatedBy.Username,
                    CreatedOn = wiki.CreatedOn,
                    ParentId = wiki.ParentId,
                    Children = currentDepth == maxDepth
                        ? new List<WikiCollectionItem>()
                        : wiki.Children.AsQueryable().Select(GetWikiProjection(maxDepth, currentDepth)).ToList()
                };

                return result;
            }
        }
    }
}
