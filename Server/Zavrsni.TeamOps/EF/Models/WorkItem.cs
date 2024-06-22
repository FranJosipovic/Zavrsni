using Zavrsni.TeamOps.EF.Enums;
using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.EF.Models
{
    public class WorkItem
    {
        public Guid Id { get; set; }
        public Guid IterationId { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? AssignedToId { get; set; }
        public Guid? ParentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkItemType Type { get; set; }
        public WorkItemPriority Priority { get; set; }
        public WorkItemStatus Status { get; set; }
        public WorkItem? Parent { get; set; }
        public ICollection<WorkItem> Children { get; } = new List<WorkItem>();
        public User CreatedBy { get; set; }
        public User? AssignedTo { get; set; }
        public Iteration Iteration { get; set; }
    }
}
