using Zavrsni.TeamOps.EF.Enums;
using Zavrsni.TeamOps.EF.Models;

namespace Zavrsni.TeamOps.Features.WorkItems.Models.DTOs
{
    public class WorkItemUSWithChildrenDTO
    {
        public Guid Id { get; set; }
        public Guid IterationId { get; set; }
        public CreatedByType CreatedBy { get; set; }
        public AssignedToType? AssignedTo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkItemPriority Priority{ get; set; }
        public WorkItemType Type { get; set; }
        public WorkItemStatus Status { get; set; }
        public List<WorkItem_Task_Bug> Children { get; set; }

        public class CreatedByType
        {
            public string Name { get; set; }
            public Guid Id { get; set; }
        }

        public class AssignedToType
        {
            public string Name { get; set; }
            public Guid? Id { get; set; }
        }
    }
}
