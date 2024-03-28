namespace Zavrsni.TeamOps.Features.Organizations.Models
{
    public class OrganizationPostModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
    }
}
