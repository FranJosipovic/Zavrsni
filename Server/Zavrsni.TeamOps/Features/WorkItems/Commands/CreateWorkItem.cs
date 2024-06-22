using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.EF.Enums;
using Zavrsni.TeamOps.EF.Models;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.Features.WorkItems.Models.DTOs;

namespace Zavrsni.TeamOps.Features.WorkItems.Commands
{
    public class CreateWorkItem
    {
        public class Command : IRequest<ServiceActionResult>
        {
            public Guid IterationId { get; set; }
            public Guid CreatedById { get; set; }
            public Guid? AssignedToId { get; set; }
            public Guid? ParentId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public WorkItemType Type { get; set; }
            public WorkItemPriority Priority { get; set; }
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

                    var newWorkItem = _mapper.Map<WorkItem>(request);

                    if (newWorkItem.Type != WorkItemType.User_Story)
                    {
                        if (newWorkItem.ParentId == null)
                        {
                            result.SetBadRequest("Bug fix or Task must have provided parent(User Story)");
                            return result;
                        }
                        else
                        {
                            var parentExists = await _db.WorkItems.AnyAsync(w => w.Id == newWorkItem.ParentId, cancellationToken: cancellationToken);
                            if (!parentExists)
                            {
                                result.SetBadRequest("Parent is non existent");
                                return result;
                            }
                            else
                            {
                                WorkItemType workItemType = await _db.WorkItems.Where(w => w.Id == newWorkItem.ParentId).Select(w => w.Type).FirstAsync(cancellationToken);
                                if (workItemType != WorkItemType.User_Story)
                                {
                                    result.SetBadRequest("Bug fix or Task can only be assigned to User Story");
                                    return result;
                                }
                            }
                        }
                    }

                    if (newWorkItem.AssignedToId != null)
                    {
                        var exists = await _db.Users.AnyAsync(u => u.Id == newWorkItem.AssignedToId, cancellationToken);
                        if (!exists)
                        {
                            result.SetBadRequest("Cannot Assign to non existent user");
                            return result;
                        }
                    }

                    var creatorExists = await _db.Users.AnyAsync(u => u.Id == newWorkItem.CreatedById, cancellationToken);
                    if (!creatorExists)
                    {
                        result.SetBadRequest("Cannot be created by non existent user");
                        return result;
                    }

                    newWorkItem.Status = WorkItemStatus.New;

                    await _db.WorkItems.AddAsync(newWorkItem, cancellationToken);
                    await _db.SaveChangesAsync(cancellationToken);

                    //create Response DTO
                    var responseModel = new WorkItemResponseItemDTO
                    {
                        AssignedTo = new WorkItemResponseItemDTO.CreatedWorkItemDTOAssignedTo
                        {
                            Id = newWorkItem.AssignedToId,
                            Name = newWorkItem.AssignedToId == null ? "UNASSIGNED" : await _db.Users.Where(u => u.Id == newWorkItem.AssignedToId).Select(u => u.Name + " " + u.Surname).FirstAsync(cancellationToken: cancellationToken),
                        },
                        CreatedBy = new WorkItemResponseItemDTO.CreatedWorkItemDTOCreatedBy
                        {
                            Id = newWorkItem.CreatedById,
                            Name = await _db.Users.Where(u => u.Id == newWorkItem.CreatedById).Select(u => u.Name + " " + u.Surname).FirstAsync(cancellationToken: cancellationToken)
                        },
                        Status = newWorkItem.Status.ToString(),
                        Type = newWorkItem.Type.ToString().Replace("_", " "),
                        Description = newWorkItem.Description,
                        Title = newWorkItem.Title,
                        Id = newWorkItem.Id,
                        IterationId = newWorkItem.IterationId,
                        Priority = newWorkItem.Priority.ToString(),
                        ParentId = newWorkItem.ParentId,
                    };
                    result.SetResultCreated(responseModel);
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
