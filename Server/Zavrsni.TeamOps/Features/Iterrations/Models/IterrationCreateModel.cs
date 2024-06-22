namespace Zavrsni.TeamOps.Features.Iterrations.Models
{
    public class IterrationCreateModel
    {
        public Guid ProjectId { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
    }
}
