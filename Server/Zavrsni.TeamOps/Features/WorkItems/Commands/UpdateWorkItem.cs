using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.EF.Enums;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Features.WorkItems.Models.DTOs;

namespace Zavrsni.TeamOps.Features.WorkItems.Commands
{
    public class UpdateWorkItem
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public Guid Id { get; set; }
            public Guid? AssignedToId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public WorkItemPriority Priority { get; set; }
            public WorkItemStatus Status { get; set; }
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

                    var workItem = await _db.WorkItems.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);

                    if (workItem == null)
                    {
                        result.SetBadRequest("Work item does not exists");
                        return result;
                    }

                    workItem.AssignedToId = request.AssignedToId;
                    workItem.Description = request.Description;
                    workItem.Title = request.Title;
                    workItem.Status = request.Status;
                    workItem.Priority = request.Priority;

                    _db.Update(workItem);
                    await _db.SaveChangesAsync(cancellationToken);

                    var updatedWorkItem = await _db.WorkItems
                        .Include(w => w.AssignedTo)
                        .Include(w => w.CreatedBy)
                        .Where(w => w.Id == request.Id)
                        .Select(w =>

                            new WorkItem_Task_Bug
                            {
                                AssignedTo = new WorkItem_Task_Bug.AssignedToType
                                {
                                    Id = w.AssignedToId,
                                    Name = w.AssignedToId == null ? "UNASSIGNED" : w.AssignedTo!.Name + ' ' + w.AssignedTo.Surname,
                                },
                                CreatedBy = new WorkItem_Task_Bug.CreatedByType
                                {
                                    Id = w.CreatedById,
                                    Name = w.CreatedBy.Name + ' ' + w.CreatedBy.Surname,
                                },
                                Status = w.Status,
                                Type = w.Type,
                                Description = w.Description,
                                Title = w.Title,
                                Id = w.Id,
                                IterationId = w.IterationId,
                            }
                        ).FirstOrDefaultAsync();

                    result.SetResultCreated(updatedWorkItem!);
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
