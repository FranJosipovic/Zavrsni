using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.EF.Models
{
    public class Iteration
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }

        public Project  Project { get; set; }
        public List<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
    }
}
