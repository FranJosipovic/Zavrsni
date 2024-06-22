using Zavrsni.TeamOps.EF.Enums;

namespace Zavrsni.TeamOps.Features.WorkItems.Models
{
    public class WorkItemUpdateModel
    {
        public Guid Id { get; set; }
        public Guid? AssignedToId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkItemPriority Priority { get; set; }
        public WorkItemStatus Status { get; set; }
    }
}
