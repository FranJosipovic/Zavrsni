using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.EF.Enums;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Features.WorkItems.Models.DTOs;

namespace Zavrsni.TeamOps.Features.WorkItems.Queries
{
    public class GetWorkItemsByIteration
    {
        public class Query : IRequest<ServiceActionResult>
        {
            public Guid IterationId { get; set; }
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

                    if(await _db.Iterations.FindAsync(new object?[] { request.IterationId }, cancellationToken: cancellationToken) == null) 
                    {
                        serviceActionResult.SetBadRequest("Iteration does not exists");
                        return serviceActionResult;
                    }

                    var userStories = await _db.WorkItems
                        .Include(w => w.CreatedBy)
                        .Include(w => w.AssignedTo)
                        .Where(w => w.IterationId == request.IterationId && w.Type == WorkItemType.User_Story)
                        .Select(w => new WorkItemUSWithChildrenDTO
                        {
                            CreatedBy = new WorkItemUSWithChildrenDTO.CreatedByType
                            {
                                Name = w.CreatedBy.Name + ' ' + w.CreatedBy.Surname,
                                Id = w.CreatedBy.Id,
                            },
                            AssignedTo = new WorkItemUSWithChildrenDTO.AssignedToType
                            {
                                Name = w.AssignedToId != null ? w.AssignedTo!.Name + ' ' + w.AssignedTo.Surname : "UNASSIGNED",
                                Id = w.AssignedToId
                            },
                            Children = new List<WorkItem_Task_Bug>(),
                            Description = w.Description,
                            Id = w.Id,
                            IterationId = request.IterationId,
                            Title = w.Title,
                            Type = w.Type,
                            Priority = w.Priority,
                            Status = w.Status,
                        }).ToListAsync();

                    foreach (var userStory in userStories)
                    {
                        var userStoryChildren = await _db.WorkItems
                        .Include(w => w.CreatedBy)
                        .Include(w => w.AssignedTo)
                        .Where(w => w.IterationId == request.IterationId && w.ParentId == userStory.Id)
                        .Select(w => new WorkItem_Task_Bug
                        {
                            CreatedBy = new WorkItem_Task_Bug.CreatedByType
                            {
                                Name = w.CreatedBy.Name + ' ' + w.CreatedBy.Surname,
                                Id = w.CreatedBy.Id,
                            },
                            AssignedTo = new WorkItem_Task_Bug.AssignedToType
                            {
                                Name = w.AssignedToId != null ? w.AssignedTo!.Name + ' ' + w.AssignedTo.Surname : "UNASSIGNED",
                                Id = w.AssignedToId
                            },
                            Description = w.Description,
                            Id = w.Id,
                            IterationId = request.IterationId,
                            Title = w.Title,
                            Type = w.Type,
                            Priority = w.Priority,
                            Status = w.Status,
                            ParentId = w.ParentId!.Value
                        }).ToListAsync();

                        userStory.Children = userStoryChildren;
                    }

                    serviceActionResult.SetOk(new CollectionResponseData<WorkItemUSWithChildrenDTO> { Items = userStories, Count = userStories.Count }, "successfully");
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
