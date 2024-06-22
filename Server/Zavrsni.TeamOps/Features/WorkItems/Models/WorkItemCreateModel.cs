using Zavrsni.TeamOps.EF.Enums;

namespace Zavrsni.TeamOps.Features.WorkItems.Models
{
    public class WorkItemCreateModel
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
}
