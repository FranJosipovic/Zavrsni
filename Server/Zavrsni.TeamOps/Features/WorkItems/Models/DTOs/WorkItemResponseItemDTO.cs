namespace Zavrsni.TeamOps.Features.WorkItems.Models.DTOs
{
    public class WorkItemResponseItemDTO
    {
        public Guid Id { get; set; }
        public Guid IterationId { get; set; }
        public Guid? ParentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CreatedWorkItemDTOCreatedBy CreatedBy { get; set; }
        public CreatedWorkItemDTOAssignedTo AssignedTo { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public class CreatedWorkItemDTOCreatedBy
        {
            public string Name { get; set; }
            public Guid Id { get; set; }
        }

        public class CreatedWorkItemDTOAssignedTo
        {
            public string Name { get; set; }
            public Guid? Id { get; set; }
        }
    }

}
