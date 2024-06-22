﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Features.WorkItems.Models.DTOs;

namespace Zavrsni.TeamOps.Features.WorkItems.Queries
{
    public class GetWorkItemsByProject
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
                    var iterationIds = projectWithIterations.Iterrations.Select(i => i.Id).ToList();
                    var workItems = await _db.WorkItems
                        .Where(w => iterationIds.Contains(w.IterationId))
                        .Select(w=>new WorkItemResponseItemDTO
                        {
                            AssignedTo = new WorkItemResponseItemDTO.CreatedWorkItemDTOAssignedTo{ Name = w.AssignedTo == null ? "UNASSIGNED":w.AssignedTo.Name + " " + w.AssignedTo.Surname, Id = w.AssignedToId },
                            CreatedBy = new WorkItemResponseItemDTO.CreatedWorkItemDTOCreatedBy { Id = w.CreatedById,Name = w.CreatedBy.Name + " " + w.CreatedBy.Surname },
                            Status = w.Status.ToString(),
                            Priority = w.Priority.ToString(),
                            Description = w.Description,
                            Title = w.Title,
                            Id = w.Id,
                            IterationId = w.IterationId,
                            ParentId = w.ParentId,
                            Type = w.Type.ToString().Replace('_',' '),
                        })
                        .ToListAsync(cancellationToken: cancellationToken);

                    serviceActionResult.SetOk(new CollectionResponseData<WorkItemResponseItemDTO> {Items = workItems,Count = workItems.Count }, "successfully");
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
