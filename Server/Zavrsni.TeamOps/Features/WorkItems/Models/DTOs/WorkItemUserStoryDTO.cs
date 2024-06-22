namespace Zavrsni.TeamOps.Features.WorkItems.Models.DTOs
{
    public class WorkItemUserStoryDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid IterationId { get; set; }
    }
}
